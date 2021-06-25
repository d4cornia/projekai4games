using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointController : MonoBehaviour
{
    public GameObject[] PFitems;

    private void Awake()
    {
        float r = Random.Range(0, 1);
        if(r * 100 < 15)
        {
            int idx = Random.Range(0, 2);
            PFitems[idx + 7].transform.position = this.gameObject.transform.position;
            PFitems[idx + 7].layer = LayerMask.NameToLayer("Collectibles");
            PFitems[idx + 7].GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            PFitems[idx + 7].GetComponent<SpriteRenderer>().sortingOrder = 0;
            Instantiate(PFitems[idx + 6]);
        }
        else
        {
            int idx = Random.Range(0, 7);
            PFitems[idx].transform.position = this.gameObject.transform.position;
            PFitems[idx].layer = LayerMask.NameToLayer("Collectibles");
            PFitems[idx].GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            PFitems[idx].GetComponent<SpriteRenderer>().sortingOrder = 0;
            Instantiate(PFitems[idx]);
        }
        Destroy(this.gameObject);
    }
}
