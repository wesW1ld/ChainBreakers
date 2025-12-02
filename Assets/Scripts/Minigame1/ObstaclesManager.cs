using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaclesManager : MonoBehaviour
{
    private static ObstaclesManager instance;
    public static ObstaclesManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("EnemyManager is NULL.");
            }
            return instance;
        }
    }

    private void Awake()
    {

        if (instance)
        {
            Debug.LogError("GameManager is already in the scene");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }


    public GameObject telegraphPrefab;

    public float startPosX = 0;
    public float startPosY = 0;
    public float vlength = 8;
    public float hlength = 15;
    public float perfectScore = 150000;

    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartWaves()
    {
        StartCoroutine(Waves());
    }

    IEnumerator Waves()
    {

        yield return new WaitForSeconds(1f);

        MakeObstacle(-3, true);
        MakeObstacle(0, true);
        MakeObstacle(3, true);


        yield return new WaitForSeconds(1f);

        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-4f, 3f), false);
        MakeObstacle(Random.Range(-6f, 7f), true);

        yield return new WaitForSeconds(1f);

        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-6f, 7f), true);
        MakeObstacle(Random.Range(-4f, 3f), false);
        MakeObstacle(Random.Range(-4f, 3f), false);

        yield return new WaitForSeconds(3f);

        scoreManager.instance.ChangeScore((int)((playerManagerM1.instance.HP / playerManagerM1.instance.maxHP) * perfectScore));
        scoreManager.instance.MinigameEnd(1);
    }

    private void MakeObstacle(float offset, bool vertical)//x between 7 and -6, y between 3 and -4 (inclusive)
    {
        if (vertical)
        {
            GameObject tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - offset, startPosY, 0), Quaternion.identity); //creates at position
            tele1.transform.localScale = new Vector3(1, vlength, 1); //changes telegraph size
        }
        else
        {
            GameObject tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - .33f, startPosY - offset, 0), Quaternion.identity); //creates at position
            tele1.transform.localScale = new Vector3(hlength, 1, 1);
        }
        
    }
}
