using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject gateOpen;
    public void openGate()
    {
        gateOpen.SetActive(true);
        gameObject.SetActive(false);
    }
}
