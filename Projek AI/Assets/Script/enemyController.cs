using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public GameObject enemyObj;
    public GameObject enemyLight;
    LayerMask playerLayerMask;
    public float maxSpeed;
    public float curAngle;
    public float range;
    public float fov;
    private static int ctr_id = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (rb == null)
        {
            ctr_id++;
            enemyObj = GameObject.Find("PF Normal Bot");
            enemyObj.name += " (" + ctr_id + ")";
            rb = enemyObj.GetComponent<Rigidbody2D>();
            enemyLight = GameObject.Find("Enemy Directional light");
            enemyLight.name += " (" + ctr_id + ")";
            enemyLight.transform.rotation = Quaternion.Euler(0, 0, 0);
            curAngle = 90;
            range = 6;
            fov = 80;
        }
    }

    void FixedUpdate()
    {
        detection();
    }

    void detection()
    {
        for (float deg = (curAngle - fov / 2); deg < (curAngle + fov / 2); deg++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
            Vector3 offset = new Vector3((float)(Mathf.Cos(Mathf.Deg2Rad * deg) * 0.50), (float)(Mathf.Sin(Mathf.Deg2Rad * deg) * 0.50), 0);
            Vector3 origin = (Vector3)this.rb.position + offset;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, (float)(range - 0.5));
            if (raycastHit2D.collider != null)
            {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Player"))
                {
                    player = otherObj;
                    Debug.Log("Got u");
                }
            }
        }
    }
}
