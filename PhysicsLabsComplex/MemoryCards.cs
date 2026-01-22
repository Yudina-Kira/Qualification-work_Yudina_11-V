using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public partial class MemoryCards : Form
    {
        #region --- Змінні ---

        private string[,] board;
        private bool[,] opened = new bool[4, 4];
        private bool finished = true;

        private string[,] current;

        private int[] pairIndexes;

        private Random rnd = new Random();

        private Cards firstCard = null;
        private string firstValue = "";
        private int firstRow = -1;
        private int firstCol = -1;

        private Cards secondCard = null;
        private string secondValue = "";
        private int secondRow = -1;
        private int secondCol = -1;

        private bool isBusy = false;

        private string folder = Path.Combine(Application.StartupPath, "MemoryCards", "Formulas-level"); //def

        private MemoryCardsRules rulesForm;

        private int countPairs;
        private int progress;
        private int secondsPassed;
        private bool timerStarted = false;

        private bool fullClose = false;

        #endregion

        public MemoryCards()
        {
            InitializeComponent();
            SetMemoryCards(folder);
            SetBoard();

            Cards[] cards = { cards1, cards2, cards3, cards4, cards5, cards6, cards7, cards8, cards9, cards10, cards11, cards12, cards13, cards14, cards15, cards16 };

            foreach (var card in cards)
                card.Click += Card_Click;

            label1.Text = "Прогрес: 0 / 8";
            label2.Text = "Спроб: 0";
            label3.Text = "Час: 0:00";

            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += gameTimer_Tick;
        }

        #region --- Setting up gameboard to play ---

        private void SetMemoryCards(string folder)
        {
            if (!Directory.Exists(folder))
            {
                MessageBox.Show("Папка MemoryCards або папка рівня гри не знайдена!");
                return;
            }

            string[] files = Directory.GetFiles(folder);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(Path.Combine(Application.StartupPath, folder).ToString() + "\\", "");
            }

            current = new string[files.Length / 2, 2];

            for (int row = 0; row < current.GetLength(0); row++)
            {
                for (int col = 0; col < current.GetLength(1); col++)
                {
                    current[row, col] = files[row * 2 + col];
                }
            }
        }

        private void SetBoard()
        {
            pairIndexes = new int[current.GetLength(0)];
            for (int i = 0; i < pairIndexes.Length; i++)
                pairIndexes[i] = i;

            Shuffle(pairIndexes);

            int[] selected = new int[8];
            Array.Copy(pairIndexes, selected, 8);

            string[] temp = new string[16];
            int k = 0;

            foreach (int index in selected)
            {
                temp[k++] = current[index, 0];
                temp[k++] = current[index, 1];
            }

            Shuffle(temp);

            board = new string[4, 4];

            int pos = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    board[row, col] = temp[pos++];
                }
            }
        }

        private void Shuffle<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        #endregion

        #region --- Game process ---

        private async void Card_Click(object sender, EventArgs e)
        {
            if (isBusy) return;

            if (!timerStarted)
            {
                secondsPassed = 0;
                gameTimer.Start();
                timerStarted = true;
            }

            Cards clicked = sender as Cards;
            int index = Convert.ToInt32(clicked.Tag);

            int row = index / 4;
            int col = index % 4;

            if (opened[row, col]) return;
            
            string cardValue = board[row, col];

            if (cardValue != null)
            {
                OpenCard(clicked, cardValue);
                opened[row, col] = true;
            }
            else return;

            if (firstCard == null)
            {
                firstCard = clicked;
                firstValue = cardValue;
                firstRow = row;
                firstCol = col;
                return;
            }

            secondCard = clicked;
            secondValue = cardValue;
            secondRow = row;
            secondCol = col;

            isBusy = true;
            countPairs++;
            label2.Text = $"Спроб: {countPairs}";

            firstValue = Normalize(firstValue, "_answer.png");
            firstValue = Normalize(firstValue, ".png");
            secondValue = Normalize(secondValue, "_answer.png");
            secondValue = Normalize(secondValue, ".png");

            if (firstValue == secondValue)
            {
                ResetSelection();
                isBusy = false;
                progress++;
                label1.Text = $"Прогрес: {progress} / 8";

                finished = true;
                foreach (bool c in opened)
                {
                    if (!c)
                    {
                        finished = false;
                        break;
                    }
                }
                if (finished)
                {
                    gameTimer.Stop();

                    var memoryCardsFinishedForm = new MemoryCardsFinished(countPairs, secondsPassed);
                    memoryCardsFinishedForm.ShowDialog();

                    if (memoryCardsFinishedForm.RestartCardsGame)
                    {
                        RestartGame();
                    }
                    else if (MemoryCardsFinished.closing)
                    {
                        fullClose = true;
                        Close();
                    }
                }
            }
            else
            {
                await Task.Delay(1000);

                firstCard.BackgroundImage = null;
                secondCard.BackgroundImage = null;

                firstCard.BackColor = Color.FromArgb(30, 140, 180);
                secondCard.BackColor = Color.FromArgb(30, 140, 180);

                opened[firstRow, firstCol] = false;
                opened[secondRow, secondCol] = false;

                ResetSelection();
                isBusy = false;
            }
        }

        private void OpenCard(Cards toShow, string filename)
        {
            string fullPath = Path.Combine(Application.StartupPath, folder, filename);

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("Файл не знайдено за шляхом: " + fullPath);
                return;
            }

            toShow.BackColor = Color.Transparent;
            toShow.BackgroundImage = Image.FromFile(fullPath);
            toShow.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private string Normalize(string str, string extra) => str.Replace(extra, "");

        private void ResetSelection()
        {
            firstCard = null;
            firstValue = null;
            secondCard = null;
            secondValue = null;
        }

        #endregion

        #region --- ContextMenuStrip Tools ---

        private void правилаГриToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rulesForm == null || rulesForm.IsDisposed)
            {
                rulesForm = new MemoryCardsRules();
                rulesForm.Show();
            }
            else
            {
                rulesForm.WindowState = FormWindowState.Normal;
                rulesForm.BringToFront();
                rulesForm.Activate();
            }
        }

        private void початиНовуГруToolStripMenuItem_Click(object sender, EventArgs e) => RestartGame();

        #endregion

        private void RestartGame()
        {
            opened = new bool[4, 4];
            firstCard = null;
            secondCard = null;
            isBusy = false;

            countPairs = 0;
            progress = 0;
            secondsPassed = 0;
            timerStarted = false;
            label1.Text = $"Прогрес: {progress} / 8";
            label2.Text = $"Спроб: {countPairs}";
            label3.Text = $"Час: {secondsPassed / 60}:{(secondsPassed % 60):00}";

            gameTimer.Stop();

            SetMemoryCards(folder);
            SetBoard();

            Cards[] cards = { cards1, cards2, cards3, cards4, cards5, cards6, cards7, cards8, cards9, cards10, cards11, cards12, cards13, cards14, cards15, cards16 };

            foreach (var card in cards)
            {
                card.BackgroundImage = null;
                card.BackColor = Color.FromArgb(30, 140, 180);
            }
        }

        private void MemoryCards_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fullClose) return;

            var gamesExitForm = new GamesExit();
            gamesExitForm.ShowDialog();
            if (!GamesExit.closing)
            {
                e.Cancel = true;
                return;
            }

            fullClose = true;

            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Dispose();
                gameTimer = null;
            }

            if (rulesForm != null && !rulesForm.IsDisposed)
                rulesForm.Close();

            Close();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            secondsPassed++;
            label3.Text = $"Час: {secondsPassed / 60}:{(secondsPassed % 60):00}";
        }
    }
}
