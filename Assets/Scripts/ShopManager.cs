using UnityEngine;
using UnityEngine.UI;
using System;
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

    public List<Chest> chests; 
    public List<Item> commonItems, uncommonItems, rareItems, epicItems, legendaryItems;

    // UI references
    public GameObject rewardPanel;
    public Text rewardText;
    public Image rewardImage;
    public Button closeRewardButton;  
    public GameObject shopMenuPanel; 
    public Button closeShopButton;  
    public Button openShopButton;   
    public Button[] chestButtons; 
    public Button claimRewardButton; 
    public Text coinsText; 
    public TapButton tapButton;

    private bool canClaimReward = true; 
    private const float claimCooldown = 24f * 60f * 60f; // 24 hours in seconds
    private DateTime lastClaimTime;

    void Start()
    {
        // Check UI references
        if (!rewardPanel || !rewardText || !rewardImage || !closeRewardButton || 
            !shopMenuPanel || !closeShopButton || !openShopButton || 
            chestButtons == null || !claimRewardButton || !coinsText)
        {
            Debug.LogError("Please assign all necessary references in the inspector.");
            return;
        }

        rewardPanel.SetActive(false);
        shopMenuPanel.SetActive(false);

        closeRewardButton.onClick.AddListener(CloseRewardPanel);
        closeShopButton.onClick.AddListener(CloseShopMenu);

        for (int i = 0; i < chestButtons.Length; i++)
        {
            int index = i;
            chestButtons[i].onClick.AddListener(() => PurchaseChest(index));
        }

        openShopButton.onClick.AddListener(OpenShopMenu);
        claimRewardButton.onClick.AddListener(ClaimReward);

        // Load saved data
        tapButton.coins = PlayerPrefs.GetInt("Coins", 1000);
        UpdateCoinDisplay();

        // Load last claim time and update claim button
        string savedTime = PlayerPrefs.GetString("LastClaimTime", string.Empty);
        if (!string.IsNullOrEmpty(savedTime))
        {
            lastClaimTime = DateTime.FromBinary(Convert.ToInt64(savedTime));
        }
        else
        {
            lastClaimTime = DateTime.MinValue;
        }

        UpdateClaimButton();
    }

    public void OpenShopMenu()
    {
        shopMenuPanel.SetActive(true);
    }

    public void CloseShopMenu()
    {
        shopMenuPanel.SetActive(false);
    }

    public void PurchaseChest(int chestIndex)
    {
        if (chestIndex < 0 || chestIndex >= chests.Count) return;

        Chest selectedChest = chests[chestIndex];

        if (tapButton.coins >= selectedChest.price)
        {
            tapButton.coins -= selectedChest.price;
            PlayerPrefs.SetInt("Coins", tapButton.coins);
            UpdateCoinDisplay();

            Item rewardedItem = GetRandomItem(selectedChest.rarityChances);

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
        float roll = UnityEngine.Random.value * 100;
        if (roll < rarityChances[0]) return commonItems[UnityEngine.Random.Range(0, commonItems.Count)];
        else if (roll < rarityChances[1]) return uncommonItems[UnityEngine.Random.Range(0, uncommonItems.Count)];
        else if (roll < rarityChances[2]) return rareItems[UnityEngine.Random.Range(0, rareItems.Count)];
        else if (roll < rarityChances[3]) return epicItems[UnityEngine.Random.Range(0, epicItems.Count)];
        else return legendaryItems[UnityEngine.Random.Range(0, legendaryItems.Count)];
    }

    void CloseRewardPanel()
    {
        rewardPanel.SetActive(false);
    }

    public void ClaimReward()
    {
        if (canClaimReward)
        {
            Debug.Log("Claiming Daily Reward...");
            tapButton.coins += 250;
            PlayerPrefs.SetInt("Coins", tapButton.coins);
            UpdateCoinDisplay();

            lastClaimTime = DateTime.Now;
            PlayerPrefs.SetString("LastClaimTime", lastClaimTime.ToBinary().ToString()); 

            canClaimReward = false;
            claimRewardButton.interactable = false;
        }
    }

    void UpdateClaimButton()
    {
        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimTime;

        if (timeSinceLastClaim.TotalSeconds >= claimCooldown)
        {
            canClaimReward = true;
            claimRewardButton.interactable = true;
        }
        else
        {
            canClaimReward = false;
            claimRewardButton.interactable = false;
        }
    }

    void UpdateCoinDisplay()
    {
        if (coinsText != null)
        {
            coinsText.text = tapButton.coins.ToString() + "\nCoins";
        }
        else
        {
            Debug.LogError("Coins Text is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        UpdateClaimButton(); 
    }
}
