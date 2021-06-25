using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class objectiveController : MonoBehaviour
{
    public GameObject reqTextGO;
    public string requirementText;
    public List<string> listNewObj;
    public List<string> finishedObj;
    public List<string> listReq; // req di dalam key

    public bool requirement()
    {
        bool flag = false;
        GameObject player = GameObject.Find("PF Player");
        List<string> getList = new List<string>();
        foreach (var item in listReq)
        {
            foreach (var pitem in player.GetComponent<playerController>().keys)
            {
                if (item == pitem)
                {
                    getList.Add(item);
                }
            }
        }

        if (listReq.Count == 0)
        {
            flag = true;
        }
        else
        {
            if (getList.Count == listReq.Count)
            {
                flag = true;
            }
        }

        return flag;
    }

    public void showTextReq()
    {
        reqTextGO.GetComponent<reqTextController>().showText();
        reqTextGO.GetComponent<TextMeshProUGUI>().text = requirementText;
    }

    // ditaro setelah requirement untuk menyelesaikan objective terpenuhi dan objective telah di selesaikan
    public void finishAndNewObjective()
    {
        GameObject player = GameObject.Find("PF Player");
        if (requirement())
        {
            List<string> temp = new List<string>();

            foreach (var item in finishedObj)
            {
                foreach (var pitem in player.GetComponent<playerController>().listObj)
                {
                    if (pitem == item)
                    {
                        temp.Add(item);
                    }
                }
            }
            foreach (var item in temp)
            {
                player.GetComponent<playerController>().listObj.Remove(item);
                GameObject objPopUp = findChild(GameObject.Find("UI"), "ObjectiveAdded");
                objPopUp.GetComponent<reqTextController>().showText();
                objPopUp.GetComponent<TextMeshProUGUI>().text = "*Objective Removed*";
            }
            foreach (var item in listNewObj)
            {
                bool flag = true;
                foreach (var pitem in player.GetComponent<playerController>().listObj)
                {
                    if (pitem == item)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    player.GetComponent<playerController>().listObj.Add(item);
                }
            }
        }
        else
        {
            // given task complete new obj
            foreach (var item in listNewObj)
            {
                bool flag = true;
                foreach (var pitem in player.GetComponent<playerController>().listObj)
                {
                    if (pitem == item)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    player.GetComponent<playerController>().listObj.Add(item);
                    GameObject objPopUp = findChild(GameObject.Find("UI"), "ObjectiveAdded");
                    objPopUp.GetComponent<reqTextController>().showText();
                    objPopUp.GetComponent<TextMeshProUGUI>().text = "*New Objective Added*";
                }
            }
            Debug.Log("Missing req");
        }
        // refresh on text
        player.GetComponent<playerController>().updateObjective();
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
