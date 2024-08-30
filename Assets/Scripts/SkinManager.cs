using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class SkinManager : MonoBehaviour
{
    
    [SerializeField] private Skin[] skins;
    [SerializeField] private TextMeshProUGUI selectButtonText;
    [SerializeField] private TextMeshProUGUI skinNameText;
    [SerializeField] private TextMeshProUGUI skinDescriptionText;
    [SerializeField] private TextMeshProUGUI skinPriceText;
    [SerializeField] private TextMeshProUGUI playerBalanceText;
    private int playerBalance;
    
    private int currentSkin = 0;
    
    private SaveData saveData;
    
    // Start is called before the first frame update
    void Start()
    {
        saveData = new SaveData(skins.Length);
        saveData.Load();

        for (int i = 0; i < saveData.ownedSkins.Count; i++)
        {
            if (saveData.ownedSkins[i]) skins[i].isOwned = true;
        }
        currentSkin = saveData.selectedSkin;
        playerBalance = saveData.totalCoins;
        playerBalanceText.text = playerBalance + " \u25cf";
        
        if (saveData.ownedSkins.Count == 0)
        {
            saveData.ownedSkins = new List<bool>(new bool[skins.Length]);
        }

        
        foreach(Skin skin in skins) skin.skinObject.SetActive(false);
        skins[currentSkin].skinObject.SetActive(true);
        UpdateSkinInfo();
        UpdateButtonText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Next() {
        skins[currentSkin].skinObject.SetActive(false);
        currentSkin++;
        if(currentSkin >= skins.Length) currentSkin = 0;
        skins[currentSkin].skinObject.SetActive(true);
        UpdateSkinInfo();
        UpdateButtonText();
    }
    
    public void Previous() {
        skins[currentSkin].skinObject.SetActive(false);
        currentSkin--;
        if(currentSkin < 0) currentSkin = skins.Length - 1;
        skins[currentSkin].skinObject.SetActive(true);
        UpdateSkinInfo();
        UpdateButtonText();
    }
    
    public void SelectSkin() {
        if (skins[currentSkin].isOwned)
        {
            saveData.Load();
            saveData.selectedSkin = currentSkin;
            saveData.Save();
        }
        else
        {
            BuySkin();
        }
        UpdateButtonText();
    }
    
    private void BuySkin() {
        if (playerBalance >= skins[currentSkin].price) {
            saveData.Load();
            playerBalance -= skins[currentSkin].price;
            skins[currentSkin].isOwned = true;
            Debug.Log(currentSkin);
            Debug.Log(saveData.ownedSkins.Count);
            saveData.ownedSkins[currentSkin] = true; 
            saveData.selectedSkin = currentSkin;
            saveData.totalCoins = playerBalance;
            saveData.Save();
            UpdateButtonText();
            UpdateSkinInfo();
        } else {
            // No
        }
    }
    
    private void UpdateButtonText() {
        if (skins[currentSkin].isOwned) {
            if (saveData.selectedSkin == currentSkin) {
                selectButtonText.text = "Selected";
            } else {
                selectButtonText.text = "Select Skin";
            }
        } else {
            selectButtonText.text = "Buy Skin";
        }
    }
    
    private void UpdateSkinInfo() {
        skinNameText.text = skins[currentSkin].name;
        skinDescriptionText.text = skins[currentSkin].description;
        skinPriceText.text = skins[currentSkin].price + " \u25cf";
        playerBalanceText.text = playerBalance + " \u25cf";

    }
}
