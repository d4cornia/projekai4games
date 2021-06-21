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

    public bool reqiurement()
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
        GameObject player = GameObject.Find("PF Player");
        if(!player.GetComponent<playerController>().listObj.Contains("Find Green Key"))
        {
            player.GetComponent<playerController>().listObj.Add("Find Green Key");
            findChild(GameObject.Find("Player UI"), "ObjectiveAdded").SetActive(true);
            CountDown(5);
        }
        Debug.Log(requirementText);

        IEnumerator CountDown(float seconds)
        {
            while (seconds > 0)
            {
                yield return new WaitForSeconds(1f);
                seconds--;
            }
            findChild(GameObject.Find("Player UI"), "ObjectiveAdded").SetActive(false);
        }
    }

    // ditaro setelah requirement untuk menyelesaikan objective terpenuhi dan objective telah di selesaikan
    public void finishAndNewObjective()
    {
        if (reqiurement())
        {
            GameObject player = GameObject.Find("PF Player");
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
            }
            foreach (var item in listNewObj)
            {
                player.GetComponent<playerController>().listObj.Add(item);
            }

            // refresh on text
            player.GetComponent<playerController>().updateObjective();
        }
        else
        {
            Debug.Log("Missing req");
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
