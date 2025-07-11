using UnityEngine;
using UnityEngine.UI;

public class SellItemMenu : MonoBehaviour
{
    public TapButton tapButton; // Reference to TapButton for coins
    public Text populationText;
    public Text coinText;

    public GameObject sellItemPanel; // Sell Item Menu Panel
    public GameObject popupPanel; // Sell Confirmation Popup Panel
    public Text popupCoinGainText; // Text inside popup showing coin gain
    public Text popupPopGainText; // Text inside popup showing population gain
    public Image popupItemImage; // Image inside popup showing item

    public Button closeSellMenuButton; // Button to close the sell menu
    public Button openSellMenuButton; // Button to open the sell menu
    public Button closePopupButton; // Button to close the sell confirmation popup
    public Button sellButton; // Button inside the popup to confirm selling

    public GameObject[] pages; // Array for item pages
    private int currentPage = 0;

    public Button nextPageButton;
    public Button prevPageButton;

    private int population = 0;

    // Item Data
    private string selectedItem;
    private int selectedCoinGain;
    private int selectedPopGain;
    private int selectedItemIndex;

    public Text[] quantityTexts; // Array for each item's quantity text
    public Sprite[] itemImages; // Holds the images for items

    void Start()
    {
        // Initialize inventory UI
        UpdateInventoryUI();
    }

    // Opens the sell menu
    public void OpenSellMenu()
    {
        sellItemPanel.SetActive(true);
        UpdateInventoryUI();
    }

    // Closes the sell menu
    public void CloseSellMenu()
    {
        sellItemPanel.SetActive(false);
    }

    // Opens the sell popup only if the item quantity is greater than 0
    public void OpenPopup(int itemIndex, int coinGain, int popGain)
    {
        string quantityText = quantityTexts[itemIndex].text;

        // Check if the quantity text starts with 'x' and contains a valid number
        if (quantityText.StartsWith("x"))
        {
            string numberPart = quantityText.Substring(1).Trim(); // Get the part after 'x' and remove spaces

            // Check if it's a valid number
            if (int.TryParse(numberPart, out int quantity) && quantity > 0)
            {
                selectedItemIndex = itemIndex;
                selectedCoinGain = coinGain;
                selectedPopGain = popGain;

                // Update the popup UI
                popupCoinGainText.text = "+" + coinGain + " Coins";
                popupPopGainText.text = "+" + popGain + " Population";
                popupItemImage.sprite = itemImages[itemIndex];

                // Show the popup
                popupPanel.SetActive(true);
            }
            else
            {
                // Log an error if the quantity is 0 or invalid
                Debug.LogError("Item " + itemIndex + " has invalid quantity text: " + quantityText);
            }
        }
        else
        {
            // Log an error if the format is incorrect (does not start with 'x')
            Debug.LogError("Item " + itemIndex + " quantity text is not in the correct format: " + quantityText);
        }
    }

    // Closes the sell popup
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    // Next & Previous Page Methods
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
        }
    }

    // OnClick Methods for 14 Items (Sell Buttons)
    public void SellBagOfRice() { OpenPopup(0, 10, 1); }
    public void SellBronzeCoin() { OpenPopup(1, 15, 2); }
    public void SellChestKey() { OpenPopup(2, 25, 3); }
    public void SellDragonEgg() { OpenPopup(3, 100, 5); }
    public void SellGoldNecklace() { OpenPopup(4, 75, 4); }
    public void SellSilverRing() { OpenPopup(5, 50, 3); }
    public void SellSword() { OpenPopup(6, 30, 2); }
    public void SellWheat() { OpenPopup(7, 5, 1); }
    public void SellKingsMail() { OpenPopup(8, 120, 6); }
    public void SellKingsScepter() { OpenPopup(9, 200, 8); }
    public void SellLeatherGloves() { OpenPopup(10, 20, 2); }
    public void SellPotion() { OpenPopup(11, 40, 3); }
    public void SellRelic() { OpenPopup(12, 90, 5); }
    public void SellRustyNail() { OpenPopup(13, 2, 0); }

    // Confirm Selling an Item
    public void SellItem()
    {
        string quantityText = quantityTexts[selectedItemIndex].text;
        if (quantityText.StartsWith("x"))
        {
            string numberPart = quantityText.Substring(1).Trim();
            if (int.TryParse(numberPart, out int quantity) && quantity > 0)
            {
                quantity--; // Reduce item quantity
                quantityTexts[selectedItemIndex].text = "x" + quantity; // Update the quantity text

                tapButton.coins += selectedCoinGain;
                population += selectedPopGain;

                UpdateInventoryUI(); // Update UI after selling
                ClosePopup();
            }
            else
            {
                // Debugging: Log that the item can't be sold as quantity is 0
                Debug.Log("Cannot sell item " + selectedItemIndex + ", quantity is 0.");
            }
        }
    }

    // Updates the UI to show correct inventory quantities
    void UpdateInventoryUI()
    {
        for (int i = 0; i < quantityTexts.Length; i++)
        {
            string currentQuantityText = quantityTexts[i].text;

            // Ensure that the text is in the correct format: "x" followed by the number
            if (!currentQuantityText.StartsWith("x"))
            {
                quantityTexts[i].text = "x0"; // Reset it to 0 if it's not formatted correctly
            }

            // If the text starts with "x", ensure the number is valid
            if (currentQuantityText.StartsWith("x"))
            {
                string numberPart = currentQuantityText.Substring(1).Trim();
                if (!int.TryParse(numberPart, out _)) 
                {
                    Debug.LogError("Invalid number format for item " + i + ": " + currentQuantityText);
                    quantityTexts[i].text = "x0"; // Set to default 0 if invalid
                }
            }
        }

        populationText.text = "Population: " + population;
        coinText.text = tapButton.coins + "\nCoins";
    }

    // Call this when gaining an item from a chest
    public void GainItem(int itemIndex, int amount)
    {
        string currentQuantityText = quantityTexts[itemIndex].text;
        if (currentQuantityText.StartsWith("x"))
        {
            string numberPart = currentQuantityText.Substring(1).Trim();
            if (int.TryParse(numberPart, out int quantity))
            {
                quantity += amount; // Add the amount to the current item quantity
                quantityTexts[itemIndex].text = "x" + quantity; // Update the quantity text
            }
            else
            {
                Debug.LogError("Failed to parse quantity for item " + itemIndex);
            }
        }

        UpdateInventoryUI(); // Update inventory UI after gaining an item
    }
}
