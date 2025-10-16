using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;

    private Vector2 direction;
    private bool firstMove;


    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        firstMove = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() //move on fixed update for better physics
    {
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }

    void OnMove(InputValue value) //calls from input system (On<action>)
    {
        direction = value.Get<Vector2>();
        if(firstMove)
        {
            firstMove = false;
            ObstaclesManager.Instance.StartWaves();
        }
    }
}
