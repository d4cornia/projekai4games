using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    public GameObject[] icons;

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
        else
        {
            Debug.Log("Ada bahan yang kurang");
        }
    }
}
