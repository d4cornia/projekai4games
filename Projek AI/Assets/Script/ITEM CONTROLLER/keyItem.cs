using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyItem : MonoBehaviour
{
    public string keyName;

    void Awake()
    {
        this.gameObject.name = keyName;
        if (keyName.Contains("Blue"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (keyName.Contains("Red"))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
