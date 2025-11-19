using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class AimMinigameControl : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Texture2D cursorTexture;
    private UnityEngine.Vector2 cursorHotspot;
    private UnityEngine.Vector2 mousePosition;

    [SerializeField]
    private Text startText;

    [SerializeField]
    private GameObject resultsScreen;

    [SerializeField]
    private Text scoreText, targetsChainedText;

    public static int score, targetsHit, check;
    public float time;
    private int targetNum;
    private UnityEngine.Vector2 targetRandomPosition;

    public float targetScale = .1f;

    // Start is called before the first frame update
    void Start()
    {
        cursorHotspot = new UnityEngine.Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

        startText.gameObject.SetActive(false);

        targetNum = 12;
        score = 0;
        targetsHit = 0;
        check = 0;
        startText.text = "Click the button to begin.";
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator GetReady()
    {
        for (int a = 3; a > 0; a--)
        {
            startText.text = "Starting in " + a.ToString();
            yield return new WaitForSeconds(1);
        }
        startText.text = "Begin!";
        yield return new WaitForSeconds(1);

        StartCoroutine("SpawnTargets");
    }

    private IEnumerator SpawnTargets()
    {
        startText.gameObject.SetActive(false);
        score = 0;
        targetsHit = 0;
        time = 12f;
        Time.timeScale = 4;

        for (int a = targetNum; a > 0; a--)
        {
            targetRandomPosition = new UnityEngine.Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
            GameObject tar = Instantiate(target, targetRandomPosition, UnityEngine.Quaternion.identity);
            tar.transform.localScale = new UnityEngine.Vector3(targetScale, targetScale, 1);


            yield return new WaitForSeconds(time);

            if (check == 1)
            {
                if (time >= 3f)
                {
                    time -= 1f;
                }
                check = 0;
            }
            else
            {
                break;
            }
        }
        Time.timeScale = 1;

        if (targetsHit >= 6)
        {
            score *= 2;
            if (targetsHit >= 12)
            {
                score += 30000;
            }
        }

        resultsScreen.SetActive(true);
        scoreText.text = "Score\n" + score;
        targetsChainedText.text = "Targets Chained\n" + targetsHit;
        scoreManager.instance.ChangeScore(score * 1000);
        scoreManager.instance.MinigameEnd(2);
    }

    public void StartGetReadyCoroutine()
    {
        resultsScreen.SetActive(false);
        startText.gameObject.SetActive(true);
        StartCoroutine("GetReady");
    }
}
