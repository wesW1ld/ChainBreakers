using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyPopup : MonoBehaviour
{
    public static BuyPopup Instance;

    public TextMeshProUGUI messageText;
    public Button yesButton;
    public Button noButton;

    private System.Action onConfirm;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string message, System.Action confirmAction)
    {
        messageText.text = message;
        onConfirm = confirmAction;

        gameObject.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            Hide();
        });

        noButton.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
