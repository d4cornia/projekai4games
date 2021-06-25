using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoomAccess : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject leftMonitor = findChild(gameObject, "LeftMonitor");
            GameObject centerMonitor = findChild(gameObject, "CenterMonitor");
            GameObject rightMonitor = findChild(gameObject, "RightMonitor");
            findChild(leftMonitor, "Highlighted").SetActive(true);
            findChild(centerMonitor, "Highlighted").SetActive(true);
            findChild(rightMonitor, "Highlighted").SetActive(true);
            other.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject leftMonitor = findChild(gameObject, "LeftMonitor");
            GameObject centerMonitor = findChild(gameObject, "CenterMonitor");
            GameObject rightMonitor = findChild(gameObject, "RightMonitor");
            findChild(leftMonitor, "Highlighted").SetActive(false);
            findChild(centerMonitor, "Highlighted").SetActive(false);
            findChild(rightMonitor, "Highlighted").SetActive(false);
            other.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
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
