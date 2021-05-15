using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class playerController : MonoBehaviour
{ 

    public Rigidbody2D rb;
    public GameObject playerLight;
    public GameObject playerObj;
    public Animator animator;
    public float speed;
    private int look;
    public float curAngle;
    public float range;
    public float fov;

    void Awake()
    {
        if (rb == null)
        {
            playerObj = GameObject.Find("PF Player");
            rb = playerObj.GetComponent<Rigidbody2D>();
            animator = playerObj.GetComponent<Animator>();
            playerLight = GameObject.Find("Player Direction light");
            playerLight.transform.rotation = Quaternion.Euler(0, 0, 0);
            range = 6;
        }
        look = 4;
    }

    private void Update()
    {
        processInput();
    }

    void FixedUpdate()
    {
        updateOrientationPlayer();
        //walkAnim();
        //spriteOrientation();
    }

    void processInput()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        rb.velocity = speed * dir;
    }


    public void updateOrientationPlayer()
    {
        Vector3 targetPosition = UtilsClass.GetWorldPositionFromUI();
        curAngle = UtilsClass.GetAngleFromVectorFloat((targetPosition - transform.position).normalized);
        playerLight.transform.rotation = Quaternion.Euler(0, 0, curAngle - 90);
    }

    /*void spriteOrientation()
    {
        if (fov.curAngle > 60 && fov.curAngle < 170) look = 1;
        else if (fov.curAngle > 170 && fov.curAngle < 250) look = 3;
        else if (fov.curAngle > 250 && fov.curAngle < 330) look = 4;
        else if (fov.curAngle < 60 || fov.curAngle > 330) look = 2;

        animator.SetInteger("Look", look);
        look = 0;
    }

    void walkAnim()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x + rb.velocity.y));
        if (Mathf.Abs(rb.velocity.x + rb.velocity.y) > 2) animator.speed = 1.3f;
        else animator.speed = 0.6f;
    }*/
}
