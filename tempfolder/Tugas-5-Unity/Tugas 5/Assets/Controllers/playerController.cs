using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class playerController : MonoBehaviour
{ 
    public Rigidbody2D rb;
    public GameObject playerObj;
    public float maxSpeed;
    private Vector2 force;

    public Light2D light;

   [SerializeField]
    public fieldOfView fov;

    void Awake(){
        if (rb == null){
            playerObj = GameObject.Find("Player");
            rb = playerObj.GetComponent<Rigidbody2D>();
            fov = Instantiate(GameObject.Find("FieldOfView").GetComponent<fieldOfView>());
            fov.name = "playerFOV";
            light = playerObj.GetComponent<Light2D>();
        }
    }

    private void Update(){
        processInput();
    }

    void FixedUpdate(){
        move();
        updateOrientationPlayer();
    }

    private float moveX, moveY;

    void processInput(){
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        float xForce = moveX * maxSpeed;
        float yForce = moveY * maxSpeed;

        force = new Vector2(xForce, yForce);
    }

    void move(){
        if (moveX == 0 && moveY == 0){
            rb.drag = 1;
        }
        rb.AddForce(force);
    }

    public void updateOrientationPlayer(){
        Vector3 targetPosition = UtilsClass.GetWorldPositionFromUI();
        fov.setAimDirection((targetPosition - transform.position).normalized);
        fov.setOrigin(transform.position);
        light.transform.rotation = Quaternion.Euler(0, 0, fov.curAngle + fov.fov * 3f);
    }
}
