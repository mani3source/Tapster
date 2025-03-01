using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class Chest
    {
        public string chestName;
        public int price;
        public float[] rarityChances; // Probabilities for rarity levels
    }

    [System.Serializable]
    public class Item
    {
        public string itemName;
        public string rarity;
        public Sprite itemImage;
    }

    public List<Chest> chests; // List of chests
    public List<Item> commonItems, uncommonItems, rareItems, epicItems, legendaryItems;

    // UI references
    public GameObject rewardPanel;
    public Text rewardText;
    public Image rewardImage;
    public Button closeRewardButton;  // Close button for the reward panel

    public GameObject shopMenuPanel; // Shop menu panel
    public Button closeShopButton;  // Close button for the shop menu
    public Button openShopButton;   // Button to open the shop menu

    public Button[] chestButtons; // Array of buttons for chests

    public Button claimRewardButton; // Button for the daily reward claim
    public Text coinsText; // Text to display the current coin balance
    public TapButton tapButton;
    //private int currentCoins = 1000; // Default starting coin value (can be set to any value)
    private bool canClaimReward = true; // Track the claim cooldown for the daily reward
    private float claimCooldown = 24f * 60f * 60f; // 24 hours in seconds
    private float lastClaimTime;

    void Start()
    {
        // Check if all the necessary references are assigned in the inspector
        if (rewardPanel == null || rewardText == null || rewardImage == null || closeRewardButton == null || 
            shopMenuPanel == null || closeShopButton == null || openShopButton == null || 
            chestButtons == null || claimRewardButton == null || coinsText == null)
        {
            Debug.LogError("Please assign all necessary references in the inspector.");
            return;
        }

        rewardPanel.SetActive(false);  // Hide the reward panel by default
        shopMenuPanel.SetActive(false);  // Hide the shop menu panel by default

        closeRewardButton.onClick.AddListener(CloseRewardPanel); // Close the reward panel
        closeShopButton.onClick.AddListener(CloseShopMenu); // Close the shop menu

        // Setting up chest buttons and their actions
        for (int i = 0; i < chestButtons.Length; i++)
        {
            int index = i;
            chestButtons[i].onClick.AddListener(() => PurchaseChest(index));
        }

        // Open Shop functionality
        openShopButton.onClick.AddListener(OpenShopMenu);

        // Initialize the daily reward system
        claimRewardButton.onClick.AddListener(ClaimReward);
        
        // Get the last claim time from PlayerPrefs (if any) and calculate the remaining cooldown
        lastClaimTime = PlayerPrefs.GetFloat("LastClaimTime", 0f);
        tapButton.coins = PlayerPrefs.GetInt("Coins", 1000); // Load the current coin count
        UpdateCoinDisplay(); // Update the UI with the correct coin amount
        UpdateClaimButton();
    }

    public void OpenShopMenu()
    {
        // Show the shop menu panel when the button is clicked
        shopMenuPanel.SetActive(true);
    }

    public void CloseShopMenu()
    {
        // Hide the shop menu panel when the close button is clicked
        shopMenuPanel.SetActive(false);
    }

    public void PurchaseChest(int chestIndex)
    {
        if (chestIndex < 0 || chestIndex >= chests.Count) return;

        Chest selectedChest = chests[chestIndex];

        // Check if the player has enough coins
        if (tapButton.coins >= selectedChest.price)
        {
            tapButton.coins -= selectedChest.price; // Deduct coins for chest purchase
            PlayerPrefs.SetInt("Coins", tapButton.coins); // Save the updated coin value
            UpdateCoinDisplay(); // Update the coin display

            // Reward the player with a random item from the chest
            Item rewardedItem = GetRandomItem(selectedChest.rarityChances);

            // Display reward
            rewardPanel.SetActive(true);
            rewardText.text = $"{rewardedItem.itemName}\n{rewardedItem.rarity}";
            rewardImage.sprite = rewardedItem.itemImage;
        }
        else
        {
            Debug.Log("Not enough coins to purchase this chest.");
        }
    }

    Item GetRandomItem(float[] rarityChances)
    {
        float roll = Random.value * 100;
        if (roll < rarityChances[0]) return commonItems[Random.Range(0, commonItems.Count)];
        else if (roll < rarityChances[1]) return uncommonItems[Random.Range(0, uncommonItems.Count)];
        else if (roll < rarityChances[2]) return rareItems[Random.Range(0, rareItems.Count)];
        else if (roll < rarityChances[3]) return epicItems[Random.Range(0, epicItems.Count)];
        else return legendaryItems[Random.Range(0, legendaryItems.Count)];
    }

    void CloseRewardPanel()
    {
        rewardPanel.SetActive(false);
    }

    public void ClaimReward()
    {
        if (canClaimReward)
        {
            // Add coins as the reward
            Debug.Log("Claiming Daily Reward...");
            tapButton.coins += 250; // Add 250 coins for daily reward
            PlayerPrefs.SetInt("Coins", tapButton.coins); // Save the updated coin count
            UpdateCoinDisplay(); // Update the coin display

            lastClaimTime = Time.time;
            PlayerPrefs.SetFloat("LastClaimTime", lastClaimTime); // Save the last claim time
            canClaimReward = false;
            UpdateClaimButton();
        }
    }

    void UpdateClaimButton()
    {
        if (!canClaimReward)
        {
            claimRewardButton.interactable = false; // Disable the button during cooldown
            float timeLeft = lastClaimTime + claimCooldown - Time.time;
            if (timeLeft <= 0)
            {
                canClaimReward = true;
                UpdateClaimButton();
            }
            else
            {
                // You can optionally show a cooldown message, but the button will be disabled
                // No text change to the button as you requested
            }
        }
        else
        {
            claimRewardButton.interactable = true; // Enable the button when cooldown is over
        }
    }

    void UpdateCoinDisplay()
    {
        if (coinsText != null)
        {
            //coinsText.text = "Coins: " + currentCoins.ToString(); // Update the coin display text
            coinsText.text = tapButton.coins.ToString() + "\nCoins";  // Format the text with a line break
        }
        else
        {
            Debug.LogError("Coins Text is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        UpdateClaimButton(); // Update the claim button every frame to check cooldown
    }
}
