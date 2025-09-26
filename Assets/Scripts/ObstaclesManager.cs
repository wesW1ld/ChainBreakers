using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    public GameObject telegraphPrefab;

    public float startPosX = 0;
    public float startPosY = 0;
    public float length = 8;

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
        GameObject tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - 3, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(1, length, 1);
        GameObject tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, length, 1);
        GameObject tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX + 3, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, length, 1);

        yield return new WaitForSeconds(5);

        tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX - 5, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(1, length, 1);
        tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX + 2, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, length, 1);
        tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX + 7, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, length, 1);

        yield return new WaitForSeconds(5);

        tele1 = Instantiate(telegraphPrefab, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
        tele1.transform.localScale = new Vector3(1, length, 1);
        tele2 = Instantiate(telegraphPrefab, new Vector3(startPosX + 2, startPosY, 0), Quaternion.identity);
        tele2.transform.localScale = new Vector3(1, length, 1);
        tele3 = Instantiate(telegraphPrefab, new Vector3(startPosX + 4, startPosY, 0), Quaternion.identity);
        tele3.transform.localScale = new Vector3(1, length, 1);
    }
}
