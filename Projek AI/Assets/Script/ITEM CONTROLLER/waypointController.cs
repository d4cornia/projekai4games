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
            PFitems[idx + 6].transform.position = this.gameObject.transform.position;
            Instantiate(PFitems[idx + 6]);
        }
        else
        {
            int idx = Random.Range(0, 5);
            PFitems[idx].transform.position = this.gameObject.transform.position;
            Instantiate(PFitems[idx]);
        }
        Destroy(this.gameObject);
    }
}
