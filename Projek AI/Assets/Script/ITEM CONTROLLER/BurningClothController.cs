using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BurningClothController : MonoBehaviour
{
    GameObject enemy;
    Rigidbody2D rb;
    Light2D lightOrange, lightYellow;
    public static int id;
    public float diO, diY;
    public int delay;

    public float range { get => (float)(lightYellow.pointLightOuterRadius - (1.9 + lightYellow.falloffIntensity * lightYellow.falloffIntensity)); }

    // Start is called before the first frame update
    void Awake()
    {
        id++;
        this.gameObject.transform.position = GameObject.Find("PF Player").GetComponent<playerController>().transform.position;
        this.gameObject.name += id;

        this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = GameObject.Find("PF Player").GetComponent<SpriteRenderer>().sortingLayerName;
        this.gameObject.layer = GameObject.Find("PF Player").layer;

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        lightOrange = GameObject.Find("Light Source Orange").GetComponent<Light2D>();
        lightOrange.name += id;
        lightYellow = GameObject.Find("Light Source Yellow").GetComponent<Light2D>();
        lightYellow.name += id;
        delay = 10;
    }

    private void Start()
    {
        lightOrange.pointLightOuterRadius = 4;
        lightOrange.intensity = (float)0.30;
        lightYellow.pointLightOuterRadius = 6;
        lightYellow.intensity = (float)0.35;
        StartCoroutine(CountDown());
    }


    // Update is called once per frame
    void Update()
    {
        lightOrange.intensity += diO;
        lightYellow.intensity += diY;
        if(lightOrange.intensity >= 0.45 || lightOrange.intensity <= 0.30)
        {
            diO *= -1;
        }
        if (lightYellow.intensity >= 0.55 || lightYellow.intensity <= 0.25)
        {
            diY *= -1;
        }
    }

    IEnumerator CountDown()
    {
        while (delay > 0)
        {
            yield return new WaitForSeconds(1f);
            delay--;
        }

        Destroy(this.gameObject);
    }

    /*void bait()
    {
        Debug.Log("Vector: " + range);
        for (float deg = 0; deg < 360; deg+=10)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
            Vector3 offset = new Vector3((float)(Mathf.Cos(Mathf.Deg2Rad * deg) * 0.50), (float)(Mathf.Sin(Mathf.Deg2Rad * deg) * 0.50), 0);
            Vector3 origin = this.gameObject.transform.position + offset;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, range);
            Debug.DrawRay(origin, direction.normalized * range, Color.green);
            if (raycastHit2D.collider != null)
            {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Player"))
                {
                    enemy = otherObj;
                    Debug.Log("Bait Weeping angel");
                    // panggil fungsi bait weeping angel untuk kesini

                }
            }
        }
    }*/
}
