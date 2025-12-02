using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Monster startingMonster;

    void Start()
    {
        startingMonster.SetActive(true);
    }
}
