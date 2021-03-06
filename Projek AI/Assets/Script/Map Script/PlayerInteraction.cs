using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private const string DOOR = "Door";
    private const string GATE = "Gate";
    private const string LIFT = "Lift";
    private bool isInBoundary = false;
    private GameObject parentDoor;
    private string tag;
    public GameObject locationText;
    public string defaultLocation;

    private void Awake()
    {
        locationText.GetComponent<TextMeshProUGUI>().text = defaultLocation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Location")
        {
            locationText.GetComponent<TextMeshProUGUI>().text = collision.gameObject.name;
        }
        else if (collision.gameObject.name == GATE)
        {
            tag = GATE;
            isInBoundary = true;
            parentDoor = collision.gameObject;
            Debug.Log(isInBoundary);
        }
        else if (collision.gameObject.name == LIFT)
        {
            tag = LIFT;
            isInBoundary = true;
            parentDoor = collision.gameObject;
            Debug.Log(isInBoundary);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GATE || collision.tag == LIFT)
        {
            tag = "";
            isInBoundary = false;
            this.parentDoor = null;
            Debug.Log(isInBoundary);
        }
    }

    private void Update()
    {
        //Gate
        if (Input.GetKeyDown(KeyCode.E) && isInBoundary)
        {
            Debug.Log(tag);
            switch (tag)
            {
                case GATE:
                    GameObject closed = findChild(parentDoor, "Closed");
                    GameObject opened = findChild(parentDoor, "Opened");
                    if (closed.activeSelf)
                    {
                        closed.SetActive(false);
                        opened.SetActive(true);
                    }
                    else
                    {
                        closed.SetActive(true);
                        opened.SetActive(false);
                    }
                    break;
                case LIFT:
                    parentDoor.GetComponent<LiftState>().openLift();
                    break;

            }

        }

    }

    private GameObject findChild(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }




}
