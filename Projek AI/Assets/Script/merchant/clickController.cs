using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clickController : MonoBehaviour, IPointerClickHandler
{
    public GameObject text;
    public GameObject merchant;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            text.GetComponent<Text>().text = (int.Parse(text.GetComponent<Text>().text) + 1) + "";
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(int.Parse(text.GetComponent<Text>().text) - 1 >= 0)
            {
                text.GetComponent<Text>().text = (int.Parse(text.GetComponent<Text>().text) - 1) + "";
            }
        }

        // count total values
        merchant.GetComponent<merchantController>().countTotal();
    }
}