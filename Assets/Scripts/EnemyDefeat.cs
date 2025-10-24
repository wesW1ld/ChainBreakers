using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDefeat : MonoBehaviour
{
    [Tooltip("This must exactly match the monster's name in the map scene")]
    public string monsterNameInMap = "Monster_Blue";

    public void OnDefeated()
    {
        Debug.Log(monsterNameInMap + " defeated!");

        // Save that this monster was beaten
        PlayerPrefs.SetInt(monsterNameInMap + "_Defeated", 1);
        PlayerPrefs.Save();

        // Return to the map scene
        SceneManager.LoadScene("LevelView");
    }
}
