using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class clickController : MonoBehaviour, IPointerClickHandler
{
    public GameObject text;
    public GameObject merchant;
    public int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        playerController playerCon = GameObject.Find("PF Player").GetComponent<playerController>();
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(GameObject.Find("transBtn").GetComponent<TextMeshProUGUI>().text.ToLower() == "sell")
            {
                if(index < 7)
                {
                    if(int.Parse(text.GetComponent<TextMeshProUGUI>().text) + 1 <= playerCon.rawItems[index])
                    {
                        text.GetComponent<TextMeshProUGUI>().text = (int.Parse(text.GetComponent<TextMeshProUGUI>().text) + 1) + "";
                    }
                    else
                    {
                        Debug.Log("Item anda tidak mencukupi untuk dijual");
                    }
                }
                else
                {
                    if (int.Parse(text.GetComponent<TextMeshProUGUI>().text) + 1 <= playerCon.items[index - 7])
                    {
                        text.GetComponent<TextMeshProUGUI>().text = (int.Parse(text.GetComponent<TextMeshProUGUI>().text) + 1) + "";
                    }
                    else
                    {
                        Debug.Log("Item anda tidak mencukupi untuk dijual");
                    }
                }
            }
            else
            {
                text.GetComponent<TextMeshProUGUI>().text = (int.Parse(text.GetComponent<TextMeshProUGUI>().text) + 1) + "";
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(int.Parse(text.GetComponent<TextMeshProUGUI>().text) - 1 >= 0)
            {
                text.GetComponent<TextMeshProUGUI>().text = (int.Parse(text.GetComponent<TextMeshProUGUI>().text) - 1) + "";
            }
        }

        // count total values
        merchant.GetComponent<merchantController>().countTotal();
    }
}