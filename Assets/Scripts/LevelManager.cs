using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public TapButton tapButton;
    public Text levelNameText;
    public Image levelImage;
    public GameObject levelInfoPanel;

    public Sprite[] levelSprites; // Level images
    public Sprite[] tapButtonSprites; // Icons for the tap button at each level

    private string[] levelNames = {
        "Starter", "Champion", "Crystal", "Explorer", "Golden",
        "Platinum", "Ultimate"
    };

    private int[] levelRequirements = { 0, 500, 1500, 5000, 15000, 50000, 150000 };

    private int currentLevel = 0;

    void Start()
    {
        LoadGame(); // Load saved level
        UpdateLevelInfo();
    }

    void Update()
    {
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        if (currentLevel < levelNames.Length - 1 && tapButton.coins >= levelRequirements[currentLevel + 1])
        {
            currentLevel++;
            UpdateLevelInfo();
            SaveGame(); // Save when leveling up
        }
    }

    void UpdateLevelInfo()
    {
        levelNameText.text = levelNames[currentLevel];
        levelImage.sprite = levelSprites[currentLevel];

        // Change the Tap Button icon when leveling up
        tapButton.UpdateButtonIcon(tapButtonSprites[currentLevel]);
    }

    public void OpenLevelInfo()
    {
        levelInfoPanel.SetActive(true);
        UpdateLevelInfo();
    }

    public void CloseLevelInfo()
    {
        levelInfoPanel.SetActive(false);
    }

    void SaveGame()
    {
        PlayerPrefs.SetInt("SavedLevel", currentLevel);
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("SavedLevel");
        }
    }

    void OnApplicationQuit()
    {
        SaveGame(); // Save when closing the game
    }
}
