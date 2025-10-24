using UnityEngine;

public class ResetProgress : MonoBehaviour
{
    // This method will be called when the Reset button is clicked
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log(" All monster progress reset!");

        // Optional: reload the current map scene so the Xs disappear immediately
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelView");
    }
}
