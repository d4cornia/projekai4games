using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keySpawner : MonoBehaviour
{
    public GameObject PFkey;
    public string keyName;
    public int delay;

    public GameObject reqTextGO;
    public string requirementText;

    public void destroyChest()
    {
        StartCoroutine(CountDown());
    }

   
    IEnumerator CountDown()
    {
        while(delay > 0)
        {
            yield return new WaitForSeconds(1f);
            delay--;
        }

        PFkey.GetComponent<objectiveController>().reqTextGO = this.reqTextGO;
        PFkey.GetComponent<objectiveController>().requirementText = this.requirementText;
        /*PFkey.GetComponent<objectiveController>().listNewObj = this.listNewObj;
        PFkey.GetComponent<objectiveController>().listReq = this.listReq;*/
        PFkey.GetComponent<objectiveController>().finishedObj = this.gameObject.GetComponent<objectiveController>().finishedObj;
        PFkey.transform.position = this.gameObject.transform.position + new Vector3(Random.Range((float)-0.3, (float)0.3), Random.Range((float)-0.3, (float)0.3), 0);
        PFkey.GetComponent<keyItem>().keyName = keyName;
        Instantiate(PFkey);
        Destroy(this.gameObject);
    }
}
