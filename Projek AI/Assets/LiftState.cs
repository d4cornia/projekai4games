using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftState : MonoBehaviour
{
    public Animator anim;
    public int isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = 0;
    }

    public void openLift()
    {
        isOpen = 1;
        anim.SetBool("isOpen", true);
    }
}
