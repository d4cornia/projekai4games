using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    GameObject enemy;
    Rigidbody2D rb;
    public static int id;
    Vector2 direction;
    public float speed, range;

    private void Awake()
    {
        id++;
        this.gameObject.name += id;
        this.gameObject.transform.position = GameObject.Find("PF Player").GetComponent<playerController>().transform.position;

        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = GameObject.Find("PF Player").GetComponent<SpriteRenderer>().sortingLayerName;
        this.gameObject.layer = GameObject.Find("PF Player").layer;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        float dir = GameObject.Find("PF Player").GetComponent<playerController>().curAngle;
        direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * dir), Mathf.Sin(Mathf.Deg2Rad * dir));
        direction.Normalize();
        speed = 2;
        range = 4;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move()
    {
        // animasi bottle dilempar
        if (rb.velocity.magnitude > 0.5)
        {

        }

        // mekanik gerak
        rb.velocity = speed * direction;
    }
}
