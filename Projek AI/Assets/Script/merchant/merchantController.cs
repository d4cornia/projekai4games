using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class merchantController : MonoBehaviour
{
    public GameObject merchant;
    public GameObject totalItemText;
    public GameObject totalCostText;
    public GameObject backPackCtr;
    public GameObject coinCtr;
    public GameObject[] qtys;
    public int[] values;
    public Animator animator;
    private int total;

    public void buySection()
    {
        GameObject.Find("buyNav").GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        GameObject.Find("sellNav").GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.25f);
        GameObject.Find("transBtn").GetComponent<TextMeshProUGUI>().text = "BUY";
        resetCart();
    }

    public void sellSection()
    {
        GameObject.Find("buyNav").GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.25f);
        GameObject.Find("sellNav").GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        GameObject.Find("transBtn").GetComponent<TextMeshProUGUI>().text = "SELL";
        resetCart();
    }

    public void action()
    {
        //transaksi
        if(GameObject.Find("transBtn").GetComponent<TextMeshProUGUI>().text.ToLower() == "buy")
        {
            if (checkBuyItems())
            {
                // jika uang player cukup maka beli item
                buyItems();
            }
        }
        else if(GameObject.Find("transBtn").GetComponent<TextMeshProUGUI>().text.ToLower() == "sell")
        {
            sellItems();
        }

        resetCart();
    }

    public bool checkBuyItems()
    {
        bool flag = true;
        int plusBackpack = 0;
        for (int i = 0; i < 10; i++)
        {
            if (i < 3)
            {
                plusBackpack += int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text);
            }
            else
            {
                plusBackpack += (int)(int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text) * 0.8);
            }
        }

        GameObject playerObj = GameObject.Find("PF Player");
        if(playerObj.GetComponent<playerController>().countBackpack() + plusBackpack > playerObj.GetComponent<playerController>().maxBackpack)
        {
            flag = false;
            Debug.Log("Your backpack is full!");
        }
        if (playerObj.GetComponent<playerController>().coin - total < 0)
        {
            flag = false;
            Debug.Log("You need more coin!");
        }

        return flag;
    }

    public void buyItems()
    {
        GameObject playerObj = GameObject.Find("PF Player");
        for (int i = 0; i < 10; i++)
        {
            if (i < 3)
            {
                playerObj.GetComponent<playerController>().items[i] += int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text);
            }
            else
            {
                playerObj.GetComponent<playerController>().rawItems[i - 3] += int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text);
            }
        }
        playerObj.GetComponent<playerController>().coin -= total;
    }

    public void sellItems()
    {
        GameObject playerObj = GameObject.Find("PF Player");
        for (int i = 0; i < 10; i++)
        {
            if (i < 3)
            {
                playerObj.GetComponent<playerController>().items[i] -= int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text);
            }
            else
            {
                playerObj.GetComponent<playerController>().rawItems[i - 3] -= int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text);
            }
        }

        // tambahkan uang player
        playerObj.GetComponent<playerController>().coin += total;
    }

    public void countTotal()
    {
        total = 0;
        int qty = 0;
        for (int i = 0; i < 10; i++)
        {
            total += (int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text) * values[i]);
            qty += int.Parse(qtys[i].GetComponent<TextMeshProUGUI>().text);
        }
        totalCostText.GetComponent<TextMeshProUGUI>().text = "Total Cost : " + total;
        totalItemText.GetComponent<TextMeshProUGUI>().text = "Total Items : " + qty;
    }
   
    public void resetCart()
    {
        // semua text di 0
        for (int i = 0; i < 10; i++)
        {
            qtys[i].GetComponent<TextMeshProUGUI>().text = "0";
        }
        //reset total
        countTotal();

        // info player di refresh
        GameObject playerObj = GameObject.Find("PF Player");
        coinCtr.GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().coin + " DRAG";
        backPackCtr.GetComponent<TextMeshProUGUI>().text = playerObj.GetComponent<playerController>().countBackpack() + " / " + playerObj.GetComponent<playerController>().maxBackpack;
        playerObj.GetComponent<playerController>().updateCtrItem();
    }

    public void openMerchant()
    {
        if (merchant.active)
        {
            merchant.SetActive(false);
        }
        else
        {
            merchant.SetActive(true);
            resetCart();
        }
    }

    private void Update()
    {
        int blinking = Mathf.FloorToInt(Random.Range(0f, 20f));
        int looking = Mathf.FloorToInt(Random.Range(0f, 50f));
        if (animator.GetCurrentAnimatorClipInfo(0).Length == 0)
        {
            if (blinking > 15)
            {
                animator.SetBool("blinking", true);
                animator.SetBool("blinking", false);
            }
            if (looking > 40)
            {
                animator.SetBool("lookingAround", true);
                animator.SetBool("lookingAround", false);
            }
        }
    }
}
