using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindObjectDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 1";
            SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = "Layer 1";
            }
        }
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 3";
            SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = "Layer 3";
            }
        }
        Debug.Log("Exit");
    }
}
