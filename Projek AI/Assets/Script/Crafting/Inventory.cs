using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory;
    public GameObject[] Slots;

    public bool[] taken;

    public void openInventory()
    {

        //inventory.SetActive(true);
        if (inventory.active)
        {
            inventory.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
            GameObject playerObj = GameObject.Find("PF Player");
            GameObject[] rawItems = new GameObject[6]
            {
            GameObject.Find("Text Battery"),
            GameObject.Find("Text Alkohol"),
            GameObject.Find("Text Cloth"),
            GameObject.Find("Text Wire"),
            GameObject.Find("Text Iron"),
            GameObject.Find("Text Bottle")
            };

            for (int i = 0; i < 6; i++)
            {
                rawItems[i].GetComponent<Text>().text = playerObj.GetComponent<playerController>().rawItems[i] + "";
            }
        }

    }
}
