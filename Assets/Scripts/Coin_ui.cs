using UnityEngine;
using TMPro; 

public class Coin_ui : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public TextMeshProUGUI coinText;

    void Update()
    {
        coinText.text = "Coins: " + playerInventory.coins;
    }
}
