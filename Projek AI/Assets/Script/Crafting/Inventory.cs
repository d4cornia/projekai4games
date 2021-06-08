using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory;

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
            GameObject[] rawItems = new GameObject[9]
            {
                GameObject.Find("Text Battery"),
                GameObject.Find("Text Alkohol"),
                GameObject.Find("Text Cloth"),
                GameObject.Find("Text Wire"),
                GameObject.Find("Text Iron"),
                GameObject.Find("Text Bottle"),
                GameObject.Find("BC Indicator"),
                GameObject.Find("DB Indicator"),
                GameObject.Find("B Indicator")
            };

            for (int i = 0; i < 9; i++)
            {
                if (i < 6)
                {
                    rawItems[i].GetComponent<Text>().text = playerObj.GetComponent<playerController>().rawItems[i] + "";
                }
                else
                {
                    rawItems[i].GetComponent<Text>().text = playerObj.GetComponent<playerController>().items[i - 6] + "";
                }
            }
            GameObject.Find("countBP").GetComponent<Text>().text = playerObj.GetComponent<playerController>().countBackpack() + "";
            GameObject.Find("Max Size").GetComponent<Text>().text = playerObj.GetComponent<playerController>().maxBackpack + "";
        }

    }
}
