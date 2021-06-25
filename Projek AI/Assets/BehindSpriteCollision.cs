using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BehindSpriteCollision : MonoBehaviour
{
    int defaultValue = 20;
    bool behind = false;
    SpriteRenderer spriteCollision;
    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "enemy")
        {
            behind = true;
            spriteCollision = collision.gameObject.GetComponent<SpriteRenderer>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "enemy")
        {
            behind = false;
            spriteCollision = collision.gameObject.GetComponent<SpriteRenderer>();
            spriteCollision.sortingOrder = defaultValue;
            findChild(gameObject, "Shadow").SetActive(false);
        }
    }

    private void Update()
    {
        if (behind)
        {
            spriteCollision.sortingOrder = sprite.sortingOrder - 1;
            findChild(gameObject, "Shadow").SetActive(true);
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
