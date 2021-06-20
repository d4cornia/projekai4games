using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public GameObject nextDoor;
    public Vector3 offset;

    public void tp()
    {
        GameObject.Find("PF Player").GetComponent<playerController>().transform.position = nextDoor.transform.position + offset;
    }
}
