using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    public GameObject[] icons;
    public GameObject[] texts;

    public void hover()
    {
        for (int i = 0; i < 3; i++)
        {
            if(i == index)
            {
                icons[i].SetActive(true);
            }
            else
            {
                icons[i].SetActive(false);
            }
        }
    }

    public void craft()
    {
        GameObject playerObj = GameObject.Find("PF Player");
        bool flag = true;
        if(index == 0)
        {
            if(playerObj.GetComponent<playerController>().rawItems[1] - 1 >= 0 && playerObj.GetComponent<playerController>().rawItems[2] - 1 >= 0)
            {
                playerObj.GetComponent<playerController>().rawItems[1]--;
                playerObj.GetComponent<playerController>().rawItems[2]--;
            }
            else
            {
                flag = false;
            }
        }
        else if(index == 1)
        {
            if (playerObj.GetComponent<playerController>().rawItems[3] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[4] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[5] - 1 >= 0)
            {
                playerObj.GetComponent<playerController>().rawItems[3] -= 2;
                playerObj.GetComponent<playerController>().rawItems[4] -= 2;
                playerObj.GetComponent<playerController>().rawItems[5]--;
            }
            else
            {
                flag = false;
            }
        }
        else if (index == 2)
        {
            if (playerObj.GetComponent<playerController>().rawItems[1] - 2 >= 0 && playerObj.GetComponent<playerController>().rawItems[2] - 2 >= 0)
            {
                playerObj.GetComponent<playerController>().rawItems[1]-=2;
                playerObj.GetComponent<playerController>().rawItems[2]-=2;
            }
            else
            {
                flag = false;
            }
        }
        if (flag)
        {
            playerObj.GetComponent<playerController>().items[index]++;
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
                if(i < 6)
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
            playerObj.GetComponent<playerController>().updateCtrItem();
        }
        else
        {
            Debug.Log("Ada bahan yang kurang");
        }
    }
}
