using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    private string[] desc = new string[3]
    {
        "burnin baby",
        "healing you",
        "Thrown to distract nearby enemies for 10 seconds."
    };
    public GameObject description;
    public GameObject[] titles;
    public GameObject[] options;
    public GameObject[] icons;
    public GameObject[] rawItems;

    public void clickNav(int idx)
    {
        index = idx;
        for (int i = 0; i < 3; i++)
        {
            if(i == index)
            {
                options[i].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
                icons[i].SetActive(true);
                description.GetComponent<TextMeshProUGUI>().text = desc[i];
            }
            else
            {
                options[i].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.25f);
                icons[i].SetActive(false);
            }
        }
        highlightTitle();
        GameObject playerObj = GameObject.Find("PF Player");
        rawItems[6].GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().items[index] + "";
        playerObj.GetComponent<Inventory>().index = index;
    }

    public void craft()
    {
        GameObject playerObj = GameObject.Find("PF Player");
        bool flag = true;
        if(index == 0)
        {
            if(playerObj.GetComponent<playerController>().rawItems[1] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[2] - 1 >= 0)
            {
                playerObj.GetComponent<playerController>().rawItems[1]-=2;
                playerObj.GetComponent<playerController>().rawItems[2]--;
            }
            else
            {
                flag = false;
            }
        }
        else if(index == 1)
        {
            if (playerObj.GetComponent<playerController>().rawItems[3] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[4] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[5] - 1 >= 0 && playerObj.GetComponent<playerController>().rawItems[6] - 1 >= 0)
            {
                playerObj.GetComponent<playerController>().rawItems[3] -= 2;
                playerObj.GetComponent<playerController>().rawItems[4] -= 2;
                playerObj.GetComponent<playerController>().rawItems[5]--;
                playerObj.GetComponent<playerController>().rawItems[6]--;
            }
            else
            {
                flag = false;
            }
        }
        else if (index == 2)
        {
            if (playerObj.GetComponent<playerController>().rawItems[1] - 1 >= 0 && playerObj.GetComponent<playerController>().rawItems[2] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[6] - 2 >= 0)
            {
                playerObj.GetComponent<playerController>().rawItems[1]--;
                playerObj.GetComponent<playerController>().rawItems[2]-=2;
                playerObj.GetComponent<playerController>().rawItems[6]--;
            }
            else
            {
                flag = false;
            }
        }
        if (flag)
        {
            playerObj.GetComponent<playerController>().items[index]++;

            for (int i = 1; i < 7; i++)
            {
                 rawItems[i - 1].GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().rawItems[i] + "";
            }
            rawItems[6].GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().items[index] + "";
            GameObject.Find("BackpackSize").GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().countBackpack() + " / " + playerObj.GetComponent<playerController>().maxBackpack;
            playerObj.GetComponent<playerController>().updateCtrItem();
        }
        else
        {
            Debug.Log("Ada bahan yang kurang");
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
