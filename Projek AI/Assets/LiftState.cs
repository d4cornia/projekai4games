using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftState : MonoBehaviour
{
    public Animator anim;
    public int isOpen;
    public Collider2D blockage;
    private int delay,max;
    public int sceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = 0;
        max = 3;
        delay = max;
    }

    public void openLift()
    {
        isOpen = 1;
        anim.SetBool("isOpen", true);
        blockage.enabled = false;
        StartCoroutine(CountDownSlow());
    }

    public IEnumerator CountDownSlow()
    {
        while (delay > 0)
        {
            yield return new WaitForSeconds(1f);
            delay--;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}
