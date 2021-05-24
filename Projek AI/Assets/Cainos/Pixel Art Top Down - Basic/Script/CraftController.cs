using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    public GameObject[] icons;

    public void hover()
    {
        for (int i = 0; i < 3; i++)
        {
            if(i == index)
            {
                icons[i].SetActive(true);
            }
            else
            {
                icons[i].SetActive(false);
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
