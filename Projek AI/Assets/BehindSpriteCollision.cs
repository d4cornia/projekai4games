using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BehindSpriteCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 2;
            findChild(gameObject, "Shadow").SetActive(true);
            Debug.Log("Enter :" + sprite.sortingLayerName);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 4;
            findChild(gameObject, "Shadow").SetActive(false);
            Debug.Log("Exit :" + sprite.sortingLayerName);
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
