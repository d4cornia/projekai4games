using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour
{
    // Start is called before the first frame update
    public void buttonPress(int index)
    {
        if (index == 1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

}
