using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class ShapesMinigameControl : MonoBehaviour
{
    [SerializeField]
    private GameObject shape1, shape2, shape3, shape4, shape5;

    [SerializeField]
    private Texture2D cursorTexture;
    private UnityEngine.Vector2 cursorHotspot;
    private UnityEngine.Vector2 mousePosition;

    [SerializeField]
    private Text startText;

    [SerializeField]
    private GameObject resultsScreen;

    [SerializeField]
    private Text scoreText, shapesChainedText;

    public static int score, shapesChained, check;
    private int shapeNum;

    private float shapeScale;
    public float time = 2f;
    // Start is called before the first frame update
    void Start()
    {
        cursorHotspot = new UnityEngine.Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

        startText.gameObject.SetActive(false);

        score = 0;
        check = 0;
        shapeNum = 15;
        shapeScale = 0.25f;
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
        yield return new WaitForSeconds(time);

        StartCoroutine("SpawnShapes");
    }
    private IEnumerator SpawnShapes()
    {
        startText.gameObject.SetActive(false);
        score = 0;
        shapesChained = 0;
        GameObject[] shapes = {shape1, shape2, shape3, shape4, shape5};

        float time = 3f;

        for (int a = shapeNum; a > 0; a--)
        {
            UnityEngine.Vector2 shapeLeftPosition = new UnityEngine.Vector2(-5, 0);
            UnityEngine.Vector2 shapeMiddlePosition = new UnityEngine.Vector2(0, 0);
            UnityEngine.Vector2 shapeRightPosition = new UnityEngine.Vector2(5, 0);
            // Generates two random shapes from the list of 5 possible shapes to be our random shapes for this roll
            int randomShapeA = Random.Range(0, 4);
            int randomShapeB = randomShapeA;
            int preventInfiniteShapeLoop = 0;
            while (randomShapeB == randomShapeA && preventInfiniteShapeLoop <= 10)
            {
                randomShapeB = Random.Range(0, 4);
                preventInfiniteShapeLoop++;
            }
            // preventInfiniteShapeLoop exists to prevent the random number generator from going on infinitely in the event of 10+ repeat rolls.
            // This is mostly here to prevent a crash in the event of the random function breaking.
            if (preventInfiniteShapeLoop >= 10)
            {
                randomShapeB = (randomShapeA + 1);
            }

            // Randomize the shape positions
            int shapePositionRandomizer = Random.Range(0, 3);
            UnityEngine.Vector2[] shapePositionArray = {shapeLeftPosition, shapeMiddlePosition, shapeRightPosition};

            // Instantiates the correct shape in a random position and then puts another shape in the other two positions
            GameObject shapeCorrect = Instantiate(shapes[randomShapeA], shapePositionArray[shapePositionRandomizer], UnityEngine.Quaternion.identity);
            GameObject shapeWrong1 = Instantiate(shapes[randomShapeB], shapePositionArray[(shapePositionRandomizer + 1) % 3], UnityEngine.Quaternion.identity);
            GameObject shapeWrong2 = Instantiate(shapes[randomShapeB], shapePositionArray[(shapePositionRandomizer + 2) % 3], UnityEngine.Quaternion.identity);
            switch (randomShapeA)
            {
                case 0:
                    Shape1.correct = 1;
                    break;
                case 1:
                    Shape2.correct = 1;
                    break;
                case 2:
                    Shape3.correct = 1;
                    break;
                case 3:
                    Shape4.correct = 1;
                    break;
                case 5:
                    Shape5.correct = 1;
                    break;
                default:
                    break;
            }


            shapeCorrect.transform.localScale = new UnityEngine.Vector3(shapeScale, shapeScale, 1);
            shapeWrong1.transform.localScale = new UnityEngine.Vector3(shapeScale, shapeScale, 1);
            shapeWrong2.transform.localScale = new UnityEngine.Vector3(shapeScale, shapeScale, 1);
            
            yield return new WaitForSeconds(time);
            if (time > 1)
            {
                time -= 0.2f;
            }

            Shape1.correct = 0;
            Shape2.correct = 0;
            Shape3.correct = 0;
            Shape4.correct = 0;
            Shape5.correct = 0;

            if (check >= 1)
            {
                Destroy(shapeCorrect);
                Destroy(shapeWrong1);
                Destroy(shapeWrong2);
                if (check == 2)
                {
                    break;
                }
                check = 0;
            }
            else
            {
                Destroy(shapeCorrect);
                Destroy(shapeWrong1);
                Destroy(shapeWrong2);
                break;
            }
        }

        if (shapesChained >= 6)
        {
            score *= 2;
            if (shapesChained >= 12)
            {
                score += 30000;
            }
        }

        resultsScreen.SetActive(true);
        scoreText.text = "Score\n" + score;
        shapesChainedText.text = "Shapes Chained\n" + shapesChained;
        scoreManager.instance.ChangeScore(score * 1000);
        scoreManager.instance.MinigameEnd(3);

        yield return new WaitForSeconds(1);
    }

    public void StartGetReadyCoroutine()
    {
        resultsScreen.SetActive(false);
        startText.gameObject.SetActive(true);
        StartCoroutine("GetReady");
    }
}
