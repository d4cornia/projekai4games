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
        speed = 3;
        range = 5;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move()
    {
        // animasi bottle dilempar

        // mekanik gerak
        rb.velocity = speed * direction;

        // pengecekkan collision dengan wall, kalo kena botol berhenti disana
        // jika sudah sampai tujuan maka pecah
        if (false)
        {
            pecah();
        }
    }
    
    void pecah()
    {
        // play sound pecah

        // bait semua bot kecuali weeping angle
        bait();

        // animasi pecah

        // destroy
        Destroy(this);
    }

    void bait()
    {
        for (float deg = 0; deg < 360; deg++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
            Vector3 offset = new Vector3((float)(Mathf.Cos(Mathf.Deg2Rad * deg) * 0.50), (float)(Mathf.Sin(Mathf.Deg2Rad * deg) * 0.50), 0);
            Vector3 origin = this.gameObject.transform.position + offset;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, range);
            if (raycastHit2D.collider != null)
            {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Player"))
                {
                    enemy = otherObj;
                    Debug.Log("Bait Sound");
                    // panggil fungsi bait setiap bot yang kena untuk kesini

                }
            }
        }
    }
}
