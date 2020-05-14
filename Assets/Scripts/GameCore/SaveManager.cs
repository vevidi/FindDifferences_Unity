using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class SaveManager
    {
        public bool SaveLoaded { get; set; } = false;
        public GameSaveModel GameSave { get; set; }
        public int SaveVersion { get { return GameSave.Version; } }

        public SaveManager()
        {
            GameSave = new GameSaveModel();
        }

        public void SaveGame(List<LevelDescriptionModel> allLevels, int version = -1)
        {
            GameSaveModel saveModel = new GameSaveModel(allLevels, version);
            GameManager.Instance.SaveManager.SaveGameInternal(saveModel);
        }

        private void SaveGameInternal(GameSaveModel gameSave, int version = -1)
        {
            int currVersion = version;
            if (currVersion == -1)
                currVersion = GameSave.Version;
            GameSave = gameSave;
            string saveJson = GameSave.Encode();
            Debug.Log("Save path -> " + Application.persistentDataPath + "/save.dat");
            if (File.Exists(Application.persistentDataPath + "/save.dat"))
                File.Delete(Application.persistentDataPath + "/save.dat");
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/save.dat");
            writer.WriteLine(saveJson);
            writer.Close();
        }

        public void LoadSave()
        {
            Debug.LogWarning("Load path -> " + Application.persistentDataPath + "/save.dat");
            if (File.Exists(Application.persistentDataPath + "/save.dat"))
            {
                StreamReader reader = new StreamReader(Application.persistentDataPath + "/save.dat");
                string json = reader.ReadLine();
                reader.Close();
                SaveLoaded = true;
                GameSave.Decode(json);
            }
        }
    }
}