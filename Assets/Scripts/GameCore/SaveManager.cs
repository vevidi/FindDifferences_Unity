using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.GameLogic
{
    public class SaveManager
    {
        public bool SaveLoaded { get; set; } = false;
        public GameSaveModel GameSave { get; set; }
        public int SaveVersion { get { return GameSave.Version; } }

        public SaveManager() => GameSave = new GameSaveModel();

        public void SaveGame(List<LevelDescriptionModel> allLevels, int version = -1, int selectedLevel = 0)
        {
            GameSaveModel saveModel = new GameSaveModel(allLevels, version, selectedLevel);
            SaveGameInternal(saveModel);
        }

        private void SaveGameInternal(GameSaveModel gameSave)
        {
            int currVersion = gameSave.Version;
            if (currVersion == -1)
            {
                currVersion = GameSave.Version;
                gameSave.Version = GameSave.Version;
            }
            GameSave = gameSave;
            string saveJson = GameSave.Encode();
            Utils.DebugLog("Save path -> " + Application.persistentDataPath + "/save.dat");
            if (File.Exists(Application.persistentDataPath + "/save.dat"))
                File.Delete(Application.persistentDataPath + "/save.dat");
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/save.dat");
            writer.WriteLine(saveJson);
            writer.Close();
        }

        public void LoadSave()
        {
            Utils.DebugLog("Load path -> " + Application.persistentDataPath + "/save.dat");
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