using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public BombManager bombManager;

    void Start()
    {
        // Add an onClick event to destroy the bomb when clicked and then play
        GetComponent<Button>().onClick.AddListener(DestroyBomb);
    }

    void DestroyBomb()
    {
        bombManager.BombClicked();
        Destroy(gameObject);
    }
}
