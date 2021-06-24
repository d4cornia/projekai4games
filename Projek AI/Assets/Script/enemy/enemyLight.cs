using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class enemyLight : MonoBehaviour
{
    public GameObject dirLight;
    public EnemyController enemyCon;
    private Light2D light;

    // Start is called before the first frame update
    void Awake()
    {
        dirLight.transform.rotation = Quaternion.Euler(0, 0, 0);
        light = dirLight.GetComponent<Light2D>();
        light.intensity = 0.1f;
        light.pointLightOuterRadius = enemyCon.coneRadius;
        light.pointLightInnerAngle = enemyCon.coneDegree;
        light.pointLightOuterAngle = enemyCon.coneDegree + 20;
    }

    void FixedUpdate()
    {
        int sign = enemyCon.rb.velocity.x > 0 ? -1 : 1; // Dapetin Polaritas
        light.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(new Vector2(0, 1), enemyCon.rb.velocity) * sign);
    }
}
