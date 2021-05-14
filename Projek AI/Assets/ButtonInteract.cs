using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public MenuButtonController menuButtonController;
    public int thisIndex;
    public AudioClip soundClick;
    public AudioSource ads;
    public GameObject menu;

    private void Awake()
    {
        ads = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            //animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                //animator.SetBool("pressed", true);
                //StartCoroutine(loadScene());
            }
        }
        else
        {
            //animator.SetBool("pressed", false);
            //animator.SetBool("selected", false);
        }
    }
    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(1);

        if (thisIndex == 1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(thisIndex);
        }
    }

    public void playSound()
    {
        ads.PlayOneShot(soundClick);
    }

}
