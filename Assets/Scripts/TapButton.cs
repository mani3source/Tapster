using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    public int coins;
    public int coinPerTap = 1;
    public Text coinText;
    public Button tapButton; // Reference to the Button component
    public Image buttonImage; // Reference to the button's image
    public Animation anim;

    void Start()
    {
        LoadGame(); // Load saved data when the game starts
    }

    public void Tap()
    {
        coins += coinPerTap;
        UpdateUI();
        anim.Play("TapAnimation");
        SaveGame(); // Save after every tap
    }

    void UpdateUI()
    {
        // Update the coin text to display as "0\nCoins"
        coinText.text = coins + "\nCoins";  // Format the text with a line break
    }

    void SaveGame()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("CoinPerTap", coinPerTap);
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinPerTap = PlayerPrefs.GetInt("CoinPerTap", 1);
        UpdateUI();
    }

    public void UpdateButtonIcon(Sprite newIcon)
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = newIcon;
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}
