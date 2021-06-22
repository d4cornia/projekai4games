using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory;
    public GameObject[] rawItems;
    public GameObject[] titles;
    public int index;

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

            highlightTitle();

            for (int i = 1; i < 7; i++)
            {
                rawItems[i - 1].GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().rawItems[i] + "";
            }
            rawItems[6].GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().items[index] + "";
            GameObject.Find("BackpackSize").GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().countBackpack() + " / " + playerObj.GetComponent<playerController>().maxBackpack;
            playerObj.GetComponent<playerController>().updateCtrItem();
        }
    }

    public void highlightTitle()
    {
        if (index == 0)
        {
            titles[0].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[1].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[2].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[3].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[4].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[5].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        }
        else if (index == 1)
        {
            titles[0].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[1].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[2].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[3].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[4].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[5].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
        }
        else if (index == 2)
        {
            titles[0].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[1].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
            titles[2].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[3].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[4].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
            titles[5].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.1f, 1f);
        }
    }
}
