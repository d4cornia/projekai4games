using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class SwitchWallDetection : MonoBehaviour
{
    public bool detection;
    //Environment
    public GameObject centerWallLayer2;
    public GameObject centerWallLayer1;

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CenterWall")
        {
            detection = !detection;
            if (detection)
            {
                centerWallLayer1.GetComponent<TilemapRenderer>().enabled = false;
                centerWallLayer1.GetComponent<ShadowCaster2D>().castsShadows = false;
                centerWallLayer2.GetComponent<TilemapRenderer>().enabled = true;
                centerWallLayer2.GetComponent<ShadowCaster2D>().castsShadows = true;
            }
            else
            {
                centerWallLayer1.GetComponent<TilemapRenderer>().enabled = true;
                centerWallLayer1.GetComponent<ShadowCaster2D>().castsShadows = true;
                centerWallLayer2.GetComponent<TilemapRenderer>().enabled = false;
                centerWallLayer2.GetComponent<ShadowCaster2D>().castsShadows = false;
            }
            Debug.Log(detection);
        }
    }
}
