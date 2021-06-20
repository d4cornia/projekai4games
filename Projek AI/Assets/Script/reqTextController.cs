using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reqTextController : MonoBehaviour
{
    public float delayText;
    float delay;


    public void showText()
    {
        gameObject.SetActive(true);
        delay = delayText;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (delay > 0)
        {
            yield return new WaitForSeconds(1f);
            delay--;
        }

        gameObject.SetActive(false);
    }
}
