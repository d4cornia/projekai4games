using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void hover(int index)
    {
        MenuButton menuButton = GameObject.Find("Button " + index).GetComponent<MenuButton>();
        menuButton.menuButtonController.index = index;
        menuButton.menuButtonController.audioSource.Play();
    }

    public void mouseClick(int index)
    {
        MenuButton menuButton = GameObject.Find(index == 0? "NewGame" : "Exit").GetComponent<MenuButton>();
        //menuButton.animator.SetBool("pressed", true);
        StartCoroutine(loadScene(menuButton));
    }

    IEnumerator loadScene(MenuButton menuButton)
    {
        yield return new WaitForSeconds(1);


        if (menuButton.thisIndex == 2)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(menuButton.thisIndex);
        }
    }
}
