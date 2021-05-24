using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BurningClothController : MonoBehaviour
{
    GameObject bc;
    GameObject enemy;
    Light2D lightOrange, lightYellow;
    public static int id;
    public float diO, diY;

    // Start is called before the first frame update
    void Awake()
    {
        id++;
        bc = GameObject.Find("PF Burning Cloth(Clone)");
        bc.transform.position = GameObject.Find("PF Player").GetComponent<playerController>().transform.position;
        bc.name += id;
        lightOrange = GameObject.Find("Light Source Orange").GetComponent<Light2D>();
        lightOrange.name += id;
        lightYellow = GameObject.Find("Light Source Yellow").GetComponent<Light2D>();
        lightYellow.name += id;
    }

    private void Start()
    {
        lightOrange.pointLightOuterRadius = 4;
        lightOrange.intensity = (float)0.5;
        lightYellow.pointLightOuterRadius = 6;
        lightYellow.intensity = (float)0.5;
    }


    // Update is called once per frame
    void Update()
    {
        lightOrange.intensity += diO;
        lightYellow.intensity += diY;
        if(lightOrange.intensity >= 0.50 || lightOrange.intensity <= 0.35)
        {
            diO *= -1;
        }
        if (lightYellow.intensity >= 0.60 || lightYellow.intensity <= 0.25)
        {
            diY *= -1;
        }
        bait();
    }

    void bait()
    {
        for (float deg = 0; deg < 360; deg++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
            Vector3 offset = new Vector3((float)(Mathf.Cos(Mathf.Deg2Rad * deg) * 0.50), (float)(Mathf.Sin(Mathf.Deg2Rad * deg) * 0.50), 0);
            Vector3 origin = this.bc.transform.position + offset;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, (float)(lightYellow.pointLightOuterRadius - (1.9 + lightYellow.falloffIntensity * lightYellow.falloffIntensity)));
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
    }
}