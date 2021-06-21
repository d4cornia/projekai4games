using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doorController : MonoBehaviour
{
    public GameObject nextDoor;
    public Vector3 offset;
    public string nextLocationName;

    public void tp()
    {
        GameObject.Find("PF Player").GetComponent<playerController>().transform.position = nextDoor.transform.position + offset;
        GameObject.Find("PF Player").GetComponent<PlayerInteraction>().locationText.GetComponent<Text>().text = nextLocationName;
    }
}
