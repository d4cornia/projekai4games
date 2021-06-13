using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Cainos.PixelArtTopDown_Basic
{
    //when object exit the trigger, put it to the assigned layer and sorting layers
    //used in the stair objects for player to travel between layers
    public class LayerTrigger : MonoBehaviour
    {
        public string layer;
        public string sortingLayer;

        private void OnTriggerExit2D(Collider2D other)
        {
            other.gameObject.layer = LayerMask.NameToLayer(layer);
            other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
            SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach ( SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = sortingLayer;
            }
            for (int i = 1; i < 4; i++)
            {
                GameObject flashlight = findChild(other.gameObject, $"Flashlight Layer {i}");
                GameObject surround = findChild(other.gameObject, $"Surroundlight Layer {i}");
                flashlight.SetActive(false);
                surround.SetActive(false);
                Debug.Log(flashlight.name + $"= Flashlight Layer {i}");
                if (flashlight.name == $"Flashlight {layer}")
                {
                    Debug.Log($"Flashlight {layer}" + " MASUKKK");
                    flashlight.SetActive(true);
                    surround.SetActive(true);
                    other.gameObject.GetComponent<playerController>().playerLight = flashlight.gameObject;
                    other.gameObject.GetComponent<playerController>().changeFlashlight();
                }
            }
            //Light2D[] lights = other.gameObject.GetComponentsInChildren<Light2D>();
            //foreach (Light2D flashlight in lights)
            //{
            //    Debug.Log(flashlight.gameObject.layer + " = " + LayerMask.NameToLayer(layer));
            //    if(flashlight.gameObject.layer == LayerMask.NameToLayer(layer))
            //    {
            //        Debug.Log("Layer|" + LayerMask.NameToLayer(layer));
            //        flashlight.gameObject.SetActive(true);
            //        if (flashlight.tag == "Flashlight") other.gameObject.GetComponent<playerController>().playerLight = flashlight.gameObject;
            //    }
            //    else
            //    {
            //        flashlight.gameObject.SetActive(false);
            //    }
            //}
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
}
