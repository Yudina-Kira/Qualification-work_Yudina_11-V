using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesLib
{
    public static class NoteDataWorking
    {
        private static string _basePath;

        public static void Initialize(string basePath)
        {
            _basePath = basePath;
        }

        private static string BasePath =>
            _basePath ?? throw new InvalidOperationException("NotesSaving is not initialized");

        // --- Base folder ---
        public static string GetSaveFolder()
        {
            string folder = Path.Combine(BasePath, "SavedNotes");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        // --- Node folder or getting its name ---
        public static string GetNodeFolder(string nodeName)
        {
            string folder = Path.Combine(GetSaveFolder(), $"{nodeName}");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        // --- Notes file name ---
        public static string GetNotesFileName()
        {
            return $"Notes {DateTime.Now:HH-mm-ss yyyy-MM-dd}.json";
        }

        // --- Clear the node folder --- 
        public static void ClearFolder(string nodeName)
        {
            string folder = GetNodeFolder(nodeName);

            foreach (var file in Directory.GetFiles(folder))
            {
                File.Delete(file);
            }
        }

        // --- Saving notes data JSON to the node folder--- 
        public static void SaveData(string nodeName, string json)
        {
            ClearFolder(nodeName);

            string path = Path.Combine(GetNodeFolder(nodeName), GetNotesFileName());

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        // --- Load notes data --- 
        public static string LoadData(string nodeName)
        {
            string folder = GetNodeFolder(nodeName);

            var files = Directory.GetFiles(folder);

            if (files.Length == 0) return null;

            return File.ReadAllText(files[0], Encoding.UTF8);
        }
    }
}
