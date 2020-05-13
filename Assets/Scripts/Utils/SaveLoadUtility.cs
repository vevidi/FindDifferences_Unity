using System.IO;
using UnityEngine;

namespace Vevidi.FindDiff.Utils
{
    public static class SaveLoadUtility
    {
        public static Texture2D LoadImage(string imageName, string imageFolder = "LoadedImages")
        {
            string path = Application.persistentDataPath + "/" + imageFolder + "/" + imageName;
            Debug.Log("Load at path: " + path);
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D newTexture = new Texture2D(1, 1);
            newTexture.LoadImage(bytes);
            newTexture.Apply();
            return newTexture;
        }

        public static void SaveImage(Texture2D image, string imageName, string imageFolder = "LoadedImages")
        {
            string folderPath = Application.persistentDataPath + "/" + imageFolder;
            Debug.Log(folderPath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            File.WriteAllBytes(folderPath + "/" + imageName, image.EncodeToJPG());
        }
    }
}