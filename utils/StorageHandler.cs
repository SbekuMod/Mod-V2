using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace SbekuMod.utils
{
    public abstract class StorageHandler<D> where D : new()
    {
        private D Data;

        public D Get() {
            if(Data == null) Initialize();

            return Data;
        }

        private static string GetSaveDirectory() => $"{Application.persistentDataPath}/{StandaloneProfileManager._saveDirectory}";

        private StandaloneProfileManager.ProfileData GetProfileData()
        {
            var profileManager = StandaloneProfileManager.SharedInstance;
            if (profileManager.currentProfile != null) return profileManager.currentProfile;

            var saveDirectory = GetSaveDirectory();
            SbekuMod.Instance.ModHelper.Console.WriteLine($"SAVE DIRECTORY: {saveDirectory}");

            StandaloneProfileManager.ProfileData loadedProfile = null;

            foreach (FileInfo fileInfo in new DirectoryInfo(saveDirectory).GetFiles("*.owprofile"))
            {
                var json = File.ReadAllText(fileInfo.FullName);
                if (json == null) continue;

                var profileData = JsonConvert.DeserializeObject<StandaloneProfileManager.ProfileData>(json);
                if (profileData == null) continue;

                if (loadedProfile == null || loadedProfile.lastModifiedTime < profileData.lastModifiedTime) loadedProfile = profileData;
            }

            return loadedProfile;
        }

        private string GetFilePath()
        {
            var currentProfile = GetProfileData();
            var saveDirectory = GetSaveDirectory();

            return Path.Combine(saveDirectory, currentProfile.profileName, GetFilename());
        }

        public void Save()
        {
            if (Data == null) Initialize();

            var filePath = GetFilePath();

            File.WriteAllText(filePath, JsonConvert.SerializeObject(Data));
        }

        public void Initialize()
        {
            Data = new D();

            var filePath = GetFilePath();
            SbekuMod.Instance.ModHelper.Console.WriteLine($"MOD SAVE FILE: {filePath}");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                Data = JsonConvert.DeserializeObject<D>(json);
            }
        }

         protected abstract string GetFilename();

    }
}
