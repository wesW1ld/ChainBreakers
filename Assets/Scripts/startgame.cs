using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [Header("Scene Name To Load")]
    public string nextScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
