using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPaper : MonoBehaviour
{
    // Start is called before the first frame update
    public string paperName;
    public GameObject canvasText;

    public void openText()
    {
        GameObject paper = findChild(canvasText, paperName);
        if (canvasText.activeSelf)
        {
            paper.SetActive(false);
            canvasText.SetActive(false);
        }
        else
        {
            paper.SetActive(true);
            canvasText.SetActive(true);
        }
    }
    private GameObject findChild(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
