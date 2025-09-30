using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegraph : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Activate());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit player");
            Destroy(gameObject);
            playerManager.instance.TakeDamage(1);
        }
    }
}
