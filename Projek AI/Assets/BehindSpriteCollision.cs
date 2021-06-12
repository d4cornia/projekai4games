using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindSpriteCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "Layer 2";
            Debug.Log("Enter :" + sprite.sortingLayerName);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "Layer 1";
            Debug.Log("Exit :" + sprite.sortingLayerName);
        }
    }
}
