using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public TapButton tapButton;
    public Text coinText;
    public Text upgradeText;
    public GameObject upgradePanel;

    private int upgradeCost = 50;
    private int coinPerTapIncrease = 1;

    public AudioSource audioSource;  // Reference to AudioSource for sound effects
    public AudioClip purchaseSound;  // Reference to the sound clip for the purchase button

    void Start()
    {
        upgradePanel.SetActive(false);
        UpdateUpgradeText();
    }

    public void BuyUpgrade()
    {
        if (tapButton.coins >= upgradeCost)
        {
            tapButton.coins -= upgradeCost;
            tapButton.coinPerTap += coinPerTapIncrease;
            upgradeCost += 25;
            UpdateUpgradeText();
            UpdateCoinText();

            // Play purchase sound
            if (audioSource != null && purchaseSound != null)
            {
                audioSource.PlayOneShot(purchaseSound);  // Play the sound when the purchase button is clicked
            }
        }
    }

    void UpdateUpgradeText()
    {
        upgradeText.text = "Upgrade Tap Power (Cost: " + upgradeCost + " Coins)";
    }

    void UpdateCoinText()
    {
        //coinText.text = "Coins: " + tapButton.coins;
        coinText.text = tapButton.coins + "\nCoins";  // Format the text with a line break
    }

    public void OpenPanel()
    {
        upgradePanel.SetActive(true);
    }

    public void ClosePanel()
    {
        upgradePanel.SetActive(false);
    }
}
