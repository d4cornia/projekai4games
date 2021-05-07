using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 600f;
    private Vector2 force;
    public Rigidbody2D rb;

    float moveX = 0f;
    float moveY = 0f;

    void Update()
    {
        processInput();
    }

    private void FixedUpdate()
    {
        move();
    }

    void processInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        float xForce = moveX * moveSpeed;
        float yForce = moveY * moveSpeed;

        force = new Vector2(xForce, yForce);
    }

    void move()
    {
        if (moveX == 0 && moveY == 0)
        {
            rb.drag = 5;
        }
        rb.AddForce(force);
    }
}
