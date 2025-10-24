using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaclesManager : MonoBehaviour
{
    public GameObject telegraphPrefab;

    public float startPosX = 0;
    public float startPosY = 0;
    public float vlength = 8;
    public float hlength = 15;
    public float perfectScore = 150000;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Waves());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Waves()
    {
        //creates a set of 3 telgraghs from the prefab
        GameObject tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - 3, startPosY, 0), Quaternion.identity); //creates at position
        tele1.transform.localScale = new Vector3(1, vlength, 1); //changes telegraph size
        GameObject tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, vlength, 1);
        GameObject tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX + 3, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, vlength, 1);

        //waits
        yield return new WaitForSeconds(1f);

        //creates more
        tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - 5, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(1, vlength, 1);
        tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX + 2, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, vlength, 1);
        tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX + 5, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, vlength, 1);
        tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(hlength, 1, 1);
        tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX - 6, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, vlength, 1);

        yield return new WaitForSeconds(1f);

        tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(1, vlength, 1);
        tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX + 2, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, vlength, 1);
        tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX + 4, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, vlength, 1);
        tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - 2, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(1, vlength, 1);
        tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX - 4, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, vlength, 1);
        tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX - 6, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, vlength, 1);

        yield return new WaitForSeconds(3f);

        scoreManager.instance.ChangeScore((int)((playerManager.instance.HP / playerManager.instance.maxHP) * perfectScore));
        FindObjectOfType<EnemyDefeat>().OnDefeated();// erase me!!!!!!!!!!!!!1
        SceneManager.LoadScene(0);
    }
}
