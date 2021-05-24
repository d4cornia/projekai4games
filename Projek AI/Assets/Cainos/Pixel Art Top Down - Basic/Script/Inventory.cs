using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

    }
}
