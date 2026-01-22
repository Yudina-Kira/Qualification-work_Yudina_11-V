using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;

namespace DvmProtocolConsole
{
    public sealed class DvmProtocol : IDisposable
    {
        public const byte HDR1 = 0x55;
        public const byte HDR2 = 0xAA;
        public const int MaxPayload = 512;

        public int BaudRate { get; set; } = 115200;
        public int DataBits { get; set; } = 8;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Handshake Handshake { get; set; } = Handshake.None;

        public int SyncTimeoutMs { get; set; } = 2000;
        public int HeaderTimeoutMs { get; set; } = 100;
        public int PayloadTimeoutMs { get; set; } = 500;
        public int CrcTimeoutMs { get; set; } = 100;

        private SerialPort _port;

        public string PortName
        {
            get
            {
                if (_port == null) return string.Empty;
                return _port.PortName;
            }
        }

        public bool IsOpen
        {
            get
            {
                return _port != null && _port.IsOpen;
            }
        }

        public DvmProtocol() { }

        public DvmProtocol(string portName) => Open(portName);

        public void Open(string portName)
        {
            Close();

            _port = new SerialPort(portName)
            {
                BaudRate = BaudRate,
                DataBits = DataBits,
                Parity = Parity,
                StopBits = StopBits,
                Handshake = Handshake,
                ReadTimeout = -1,   // керуємо тайм-аутами вручну
                WriteTimeout = 100
            };
            _port.Open();
        }

        public void Close()
        {
            try
            {
                if (_port != null)
                {
                    if (_port.IsOpen) _port.Close();
                    _port.Dispose();
                }
            }
            finally
            {
                _port = null;
            }
        }

        public void Dispose() => Close();

        // ---- Публічні команди ----
        public bool SendSimpleCmd(byte type) =>
            EnsureOpen() && WriteAll(BuildSimplePacket(type));

        public bool SendSetFs(byte code) =>
            EnsureOpen() && WriteAll(BuildSetFsPacket(code));

        public bool ReadFrame(out byte type, out byte[] payload)
        {
            type = 0;
            payload = Array.Empty<byte>();
            if (!EnsureOpen()) return false;

            // sync HDR1/HDR2
            byte prev = 0;
            var one = new byte[1];

            // прочитати перший байт, щоб ініціалізувати "prev"
            if (!ReadExact(one, 1, SyncTimeoutMs)) return false;
            prev = one[0];

            for (;;)
            {
                if (!ReadExact(one, 1, SyncTimeoutMs)) return false;
                byte b = one[0];
                if (prev == HDR1 && b == HDR2) break;
                prev = b;
            }

            // TYPE + LEN
            var hdr = new byte[3];
            if (!ReadExact(hdr, 3, HeaderTimeoutMs)) return false;

            type = hdr[0];
            ushort len = (ushort)(hdr[1] | (hdr[2] << 8));
            if (len > MaxPayload) return false;

            if (len > 0)
            {
                payload = new byte[len];
                if (!ReadExact(payload, len, PayloadTimeoutMs)) return false;
            }
            else
            {
                payload = Array.Empty<byte>();
            }

            // CRC (LE)
            var cr = new byte[2];
            if (!ReadExact(cr, 2, CrcTimeoutMs)) return false;
            ushort rxcrc = (ushort)(cr[0] | (cr[1] << 8));

            // CRC перевірка
            ushort crc = 0xFFFF;
            crc = Crc16Update(crc, type);
            crc = Crc16Update(crc, (byte)(len & 0xFF));
            crc = Crc16Update(crc, (byte)(len >> 8));
            for (int i = 0; i < len; i++)
                crc = Crc16Update(crc, payload[i]);

            return crc == rxcrc;
        }

        public bool StartSingleAndWaitForData(DvmProtocol dvm, out byte[] dataPayload)
        {
            dataPayload = null;

            // 1) отправляем команду START_SINGLE (0x80)
            if (!dvm.SendSimpleCmd(0x80))
            {
                Console.WriteLine("Не вдалось відправити CMD 0x80");
                return false;
            }

            Console.WriteLine("CMD 0x80 відправлено, очікую ACK и DATA...");

            for (int attempt = 0; attempt < 200; attempt++) // до ~2 сек
            {
                if (dvm.ReadFrame(out byte type, out byte[] payload))
                {
                    if (type == 0x13)
                    {
                        Console.WriteLine("ACK отримано (0x13). Очікую дані...");
                    }
                    else if (type == 0x10)
                    {
                        Console.WriteLine("Отримано DATA!");
                        dataPayload = payload;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("У процесі отримання даних...");
                        Console.WriteLine($"Frame type=0x{type:X2} len={payload.Length}");
                    }
                }

                Thread.Sleep(10);
            }

            Console.WriteLine("Тайм-аут: DATA (0x10) не отримано.");
            return false;
        }

        public void SeparateChannels(byte[] payload, out ushort[] ch1, out ushort[] ch2)
        {
            ch1 = Array.Empty<ushort>();
            ch2 = Array.Empty<ushort>();

            if (payload == null || payload.Length < 5)
            {
                Console.WriteLine("Payload надто короткий! (< 5)");
                return;
            }

            Console.WriteLine($"payload.Length = {payload.Length}");

            byte N = payload[2];
            Console.WriteLine($"N = {N}");

            int bytesAvailable = payload.Length - 5;
            Console.WriteLine($"bytesAvailable = {bytesAvailable}");

            int pairsTotal = Math.Min(N, bytesAvailable / 4);
            Console.WriteLine($"pairsTotal = {pairsTotal}");

            ch1 = new ushort[pairsTotal];
            ch2 = new ushort[pairsTotal];

            int offset = 5;
            ushort d = 29;
            for (int i = 0; i < pairsTotal; i++)
            {
                ch1[i] = ReadU16LE(payload, offset);
                ch1[i] -= d;
                ch2[i] = ReadU16LE(payload, offset + 2);
                offset += 4;
            }

            Console.WriteLine("ch1:");
            Console.WriteLine(string.Join(" ", ch1));
            Console.WriteLine("ch2:");
            Console.WriteLine(string.Join(" ", ch2));

        }

        public static ushort ReadU16LE(byte[] payload, int i) => (ushort)(payload[i] | (payload[i + 1] << 8));

        // ---- Приватні утиліти ----
        private bool EnsureOpen() => _port?.IsOpen == true;

        private bool WriteAll(byte[] data)
        {
            try
            {
                if (_port == null) return false;

                _port.Write(data, 0, data.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ReadExact(byte[] buffer, int n, int timeoutMs)
        {
            int got = 0;
            var sw = Stopwatch.StartNew();
            try
            {
                while (got < n)
                {
                    int available = _port.BytesToRead;

                    if (available > 0)
                    {
                        int want = Math.Min(n - got, available);
                        int rd = _port.Read(buffer, got, want);
                        got += rd;
                    }
                    else
                    {
                        if (sw.ElapsedMilliseconds > timeoutMs)
                            return false;

                        Thread.Sleep(1);
                    }

                    if (sw.ElapsedMilliseconds > timeoutMs)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static ushort Crc16Update(ushort crc, byte b)
        {
            crc ^= (ushort)(b << 8);
            for (int i = 0; i < 8; i++)
                crc = (ushort)(((crc & 0x8000) != 0) ? ((crc << 1) ^ 0x1021) : (crc << 1));
            return crc;
        }

        private static byte[] BuildSimplePacket(byte type)
        {
            ushort crc = 0xFFFF;
            crc = Crc16Update(crc, type);
            crc = Crc16Update(crc, 0x00);
            crc = Crc16Update(crc, 0x00);

            return new byte[]
            {
                HDR1, HDR2, type, 0x00, 0x00,
                (byte)(crc & 0xFF), (byte)(crc >> 8)
            };
        }

        private static byte[] BuildSetFsPacket(byte code)
        {
            ushort crc = 0xFFFF;
            crc = Crc16Update(crc, 0x83);
            crc = Crc16Update(crc, 0x01);
            crc = Crc16Update(crc, 0x00);
            crc = Crc16Update(crc, code);

            return new byte[]
            {
                HDR1, HDR2, 0x83, 0x01, 0x00, code,
                (byte)(crc & 0xFF), (byte)(crc >> 8)
            };
        }
    }
}
