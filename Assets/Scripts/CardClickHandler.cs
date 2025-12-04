using UnityEngine;
using UnityEngine.EventSystems;
using ChainBreakers;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Card cardData;
    public GameObject cardObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        // --- 1. ENSURE PLAYER HAS A COINS VARIABLE ---
        PlayerManager player = PlayerManager.instance;

        if (!player.HasCurrencyInitialized)
        {
            player.Coins = 5000;
            player.HasCurrencyInitialized = true;
            Debug.Log("Coins did not exist — initializing to 5000.");
        }

        BuyPopup.Instance.Show(
            $"Buy {cardData.cardName} for 2000 coins?",
            () =>
            {
                if (player.Coins >= 2000)
                {
                    player.Coins -= 2000;

                    Destroy(cardObject);

                    // ADD TO USER'S DECK HERE:
                    // DeckManager.instance.AddCard(cardData);

                    Debug.Log($"{cardData.cardName} purchased!");
                }
                else
                {
                    Debug.Log("Not enough coins!");
                }
            }
        );
    }
}
