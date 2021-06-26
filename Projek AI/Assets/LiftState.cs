using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftState : MonoBehaviour
{
    public GameObject nextLift;
    public Vector3 offset;
    public string nextLocationName;

    public Animator anim;
    public Collider2D blockage;
    private int delay,max;

    // Start is called before the first frame update
    void Start()
    {
        max = 3;
    }

    public void openLift()
    {
        delay = max;
        anim.SetBool("isOpen", true);
        blockage.enabled = false;
        StartCoroutine(CountDownOpen());
    }

    public void closeLift()
    {
        delay = 0;
        anim.SetBool("isOpen", false);
        blockage.enabled = true;
    }

    public IEnumerator CountDownOpen()
    {
        nextLift.GetComponent<LiftState>().anim.SetBool("isOpen", true);
        nextLift.GetComponent<LiftState>().blockage.enabled = false;
        while (delay > 0)
        {
            yield return new WaitForSeconds(1f);
            delay--;
        }

        GameObject.Find("PF Player").GetComponent<playerController>().transform.position = nextLift.transform.position + offset;
        GameObject.Find("PF Player").GetComponent<PlayerInteraction>().locationText.GetComponent<TextMeshProUGUI>().text = nextLocationName;
        closeLift();
        StartCoroutine(CountDownClose());
        if (gameObject.GetComponent<enemySpawn>() != null)
        {
            gameObject.GetComponent<enemySpawn>().unhide();
        }
    }

    public IEnumerator CountDownClose()
    {
        while (delay > 0)
        {
            yield return new WaitForSeconds(1f);
            delay--;
        }
        nextLift.GetComponent<LiftState>().closeLift();
    }
}
