using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedController : MonoBehaviour
{
    public GameObject[] itemUi;
    public GameObject playerObj;
    public GameObject[] activeItems;

    public void hoverIn()
    {
        itemUi[0].SetActive(false);
        itemUi[1].SetActive(true);
        setActive();
    }

    public void hoverOut()
    {
        itemUi[0].SetActive(true);
        itemUi[1].SetActive(false);
        setActive();
    }

    public void setActive()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == playerObj.GetComponent<playerController>().idxItem)
            {
                activeItems[i].SetActive(true);
            }
            else
            {
                activeItems[i].SetActive(false);
            }
        }
    }

    public void changeSelected(int idx)
    {
        playerObj.GetComponent<playerController>().idxItem = idx;
        setActive();
    }
}
