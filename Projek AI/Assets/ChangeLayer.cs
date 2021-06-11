using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite.sortingLayerName == "Layer 1") sprite.sortingLayerName = "Layer 2";
            else sprite.sortingLayerName = "Layer 1";
            Debug.Log(sprite.sortingLayerName);
        }
        Debug.Log("Through Gate");
    }
}
