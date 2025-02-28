using UnityEngine;
using UnityEngine.UI;

public class KingdomMenu : MonoBehaviour
{
    public TapButton tapButton; // Reference to TapButton for coins
    public Text populationText;
    public Text coinText;
    public GameObject kingdomPanel;
    public Button nextPageButton, prevPageButton;

    public GameObject page1, page2; // Pages for items
    private int currentPage = 1;

    private int population = 0;

    // Building Costs
    private int houseCost = 1000;
    private int castleCost = 2500;
    private int farmCost = 900;
    private int schoolCost = 1500;
    private int marketCost = 1200;
    private int barracksCost = 2000;

    void Start()
    {
        LoadGame(); // Load saved data
        UpdateUI();
        ShowPage(1);
        kingdomPanel.SetActive(false); // Hide the panel on start
    }

    public void BuyHouse()
    {
        if (tapButton.coins >= houseCost)
        {
            tapButton.coins -= houseCost;
            population += 5;
            SaveGame();
            UpdateUI();
        }
    }

    public void BuyCastle()
    {
        if (tapButton.coins >= castleCost)
        {
            tapButton.coins -= castleCost;
            population += 15;
            SaveGame();
            UpdateUI();
        }
    }

    public void BuyFarm()
    {
        if (tapButton.coins >= farmCost)
        {
            tapButton.coins -= farmCost;
            population += 3;
            SaveGame();
            UpdateUI();
        }
    }

    public void BuySchool()
    {
        if (tapButton.coins >= schoolCost)
        {
            tapButton.coins -= schoolCost;
            population += 10;
            SaveGame();
            UpdateUI();
        }
    }

    public void BuyMarket()
    {
        if (tapButton.coins >= marketCost)
        {
            tapButton.coins -= marketCost;
            population += 4;
            SaveGame();
            UpdateUI();
        }
    }

    public void BuyBarracks()
    {
        if (tapButton.coins >= barracksCost)
        {
            tapButton.coins -= barracksCost;
            population += 8;
            SaveGame();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        populationText.text = "Population: " + population;
        coinText.text = "Coins: " + tapButton.coins;
    }

    void SaveGame()
    {
        PlayerPrefs.SetInt("Coins", tapButton.coins);
        PlayerPrefs.SetInt("Population", population);
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            tapButton.coins = PlayerPrefs.GetInt("Coins");
        }

        if (PlayerPrefs.HasKey("Population"))
        {
            population = PlayerPrefs.GetInt("Population");
        }
    }

    public void ShowPage(int page)
    {
        currentPage = page;
        page1.SetActive(page == 1);
        page2.SetActive(page == 2);

        prevPageButton.interactable = (page > 1);
        nextPageButton.interactable = (page < 2);
    }

    public void NextPage()
    {
        if (currentPage == 1) ShowPage(2);
    }

    public void PrevPage()
    {
        if (currentPage == 2) ShowPage(1);
    }

    public void OpenPanel()
    {
        kingdomPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        kingdomPanel.SetActive(false);
    }
    void OnApplicationQuit()
    {
        SaveGame();
    }

}
