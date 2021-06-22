using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class merchantController : MonoBehaviour
{
    public GameObject merchant;
    public GameObject acationText;
    public GameObject totalText;
    public GameObject[] qtys;
    public int[] values;
    public Animator animator;

    public void buySection()
    {
        resetCart();
        GameObject.Find("section info").GetComponent<Text>().text = "Buy Section";
        acationText.GetComponent<Text>().text = "Buy";
    }

    public void sellSection()
    {
        resetCart();
        GameObject.Find("section info").GetComponent<Text>().text = "Sell Section";
        acationText.GetComponent<Text>().text = "Sell";
    }

    public void action()
    {
        //transaksi
        if(acationText.GetComponent<Text>().text == "Buy")
        {
            if (checkBuyItems())
            {
                // jika uang player cukup maka beli item
                buyItems();
            }
        }
        else if(acationText.GetComponent<Text>().text == "Sell")
        {
            if (checkSellItem())
            {
                // jika barang yang dijual ada stoknya diplayer maka berhasil jual
                sellItems();
            }
        }

        resetCart();
    }

    public bool checkBuyItems()
    {
        bool flag = true;
        int plusBackpack = 0;
        for (int i = 0; i < 9; i++)
        {
            if (i < 3)
            {
                plusBackpack += int.Parse(qtys[i].GetComponent<Text>().text);
            }
            else
            {
                plusBackpack += (int)(int.Parse(qtys[i].GetComponent<Text>().text) * 0.8);
            }
        }

        GameObject playerObj = GameObject.Find("PF Player");
        if(playerObj.GetComponent<playerController>().countBackpack() + plusBackpack > playerObj.GetComponent<playerController>().maxBackpack)
        {
            flag = false;
            Debug.Log("Your backpack is full!");
        }
        if (playerObj.GetComponent<playerController>().money - int.Parse(totalText.GetComponent<Text>().text) < 0)
        {
            flag = false;
            Debug.Log("You need more money!");
        }

        return flag;
    }

    public void buyItems()
    {
        GameObject playerObj = GameObject.Find("PF Player");
        for (int i = 0; i < 9; i++)
        {
            if (i < 3)
            {
                playerObj.GetComponent<playerController>().items[i] += int.Parse(qtys[i].GetComponent<Text>().text);
            }
            else
            {
                playerObj.GetComponent<playerController>().rawItems[i - 3] += int.Parse(qtys[i].GetComponent<Text>().text);
            }
        }
        playerObj.GetComponent<playerController>().money -= int.Parse(totalText.GetComponent<Text>().text);
    }

    public bool checkSellItem()
    {
        bool flag = true;
        GameObject playerObj = GameObject.Find("PF Player");
        for (int i = 0; i < 9; i++)
        {
            if(i < 3)
            {
                if (playerObj.GetComponent<playerController>().items[i] - int.Parse(qtys[i].GetComponent<Text>().text) < 0)
                {
                    flag = false;
                }
            }
            else
            {
                if (playerObj.GetComponent<playerController>().rawItems[i - 3] - int.Parse(qtys[i].GetComponent<Text>().text) < 0)
                {
                    flag = false;
                }
            }
        }

        return flag;
    }

    public void sellItems()
    {
        GameObject playerObj = GameObject.Find("PF Player");
        for (int i = 0; i < 9; i++)
        {
            if (i < 3)
            {
                playerObj.GetComponent<playerController>().items[i] -= int.Parse(qtys[i].GetComponent<Text>().text);
            }
            else
            {
                playerObj.GetComponent<playerController>().rawItems[i - 3] -= int.Parse(qtys[i].GetComponent<Text>().text);
            }
        }

        // tambahkan uang player
        playerObj.GetComponent<playerController>().money += int.Parse(totalText.GetComponent<Text>().text);
    }

    public void countTotal()
    {
        int total = 0;
        for (int i = 0; i < 9; i++)
        {
            total += (int.Parse(qtys[i].GetComponent<Text>().text) * values[i]);
        }
        totalText.GetComponent<Text>().text = total + "";
    }
   
    public void resetCart()
    {
        // semua text di 0
        for (int i = 0; i < 9; i++)
        {
            qtys[i].GetComponent<Text>().text = "0";
        }
        //reset total
        countTotal();

        // info player di refresh
        GameObject playerObj = GameObject.Find("PF Player");
        GameObject.Find("money ctr").GetComponent<Text>().text = playerObj.GetComponent<playerController>().money + "";
        GameObject.Find("countBP").GetComponent<Text>().text = playerObj.GetComponent<playerController>().countBackpack() + "";
        GameObject.Find("Max Size").GetComponent<Text>().text = playerObj.GetComponent<playerController>().maxBackpack + "";
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
