using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public string sceneName; // The scene this monster leads to
    public Monster[] connectedMonsters; // The next monsters in the path
    public GameObject redX; // UI Image for the red X (child of this object)

    private bool defeated = false;
    private bool active = false;
    private Button button;

    void Start()
    {
        // If this monster was previously defeated, mark it and unlock its children
        if (PlayerPrefs.GetInt(gameObject.name + "_Defeated", 0) == 1)
        {
            Defeat();
        }
    }

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        active = value;
        button.interactable = value;
        var img = GetComponent<Image>();
        img.color = value ? Color.white : new Color(1f, 1f, 1f, 0.4f);
    }

    void OnClick()
    {
        if (!active || defeated) return;
        SceneManager.LoadScene(sceneName);
    }

    public void Defeat()
    {
        defeated = true;
        if (redX != null) redX.SetActive(true);
        SetActive(false);
        foreach (Monster next in connectedMonsters)
        {
            next.SetActive(true);
        }
    }
}
