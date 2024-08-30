using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Utilities
{
    using UnityEngine;

    [System.Serializable]
    public class SaveData
    {
        public int totalCoins;
        public float highScore;
        public int selectedSkin;
        public List<bool> ownedSkins;

        public SaveData(int skinCount = 0)
        {
            totalCoins = 0;
            highScore = 0;
            selectedSkin = 0;
            ownedSkins = new List<bool>(new bool[skinCount]);
        }
        
        public void Save()
        {
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.SetInt("CurrentSkin", selectedSkin);
            
            for (int i = 0; i < ownedSkins.Count; i++)
            {
                PlayerPrefs.SetInt("OwnedSkin_" + i, ownedSkins[i] ? 1 : 0);
            }
            
            PlayerPrefs.Save();
        }

        public void Load()
        {
            totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            highScore = PlayerPrefs.GetFloat("HighScore", 0);
            selectedSkin = PlayerPrefs.GetInt("CurrentSkin", 0);
            
            for (int i = 0; i < ownedSkins.Count; i++)
            {
                ownedSkins[i] = PlayerPrefs.GetInt("OwnedSkin_" + i, 0) == 1;
            }
        }
    }
}