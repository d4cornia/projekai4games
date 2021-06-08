using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class merchantController : MonoBehaviour
{
    public GameObject merchant;
    public GameObject acationText;

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

        resetCart();
    }

    public void resetCart()
    {
        // semua text di 0


        // info player di refresh
        GameObject playerObj = GameObject.Find("PF Player");
        GameObject.Find("money ctr").GetComponent<Text>().text = playerObj.GetComponent<playerController>().money + "";
        GameObject.Find("countBP").GetComponent<Text>().text = playerObj.GetComponent<playerController>().countBackpack() + "";
        GameObject.Find("Max Size").GetComponent<Text>().text = playerObj.GetComponent<playerController>().maxBackpack + "";
    }

    public void openMercahnt()
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
}
