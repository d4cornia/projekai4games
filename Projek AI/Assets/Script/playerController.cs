using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject playerLight;
    public GameObject playerObj;
    public GameObject inventory;
    public Animator animator;
    private int look;

    // stats
    public float health;
    public float speed;
    public float curAngle;
    public float range;
    public float fov;
    public int idxItem;
    public int maxBackpack;

    // flashlight
    Light2D lightPlayer;
    bool fstate;


    // prefab item
    public GameObject[] PFitem;

    //item
    bool flagc;
    public int[] items;
    // 0 : burning cloth
    // 1 : bottle
    // 2 : Health

    // raw item
    public int[] rawItems;
    // 0 : batrei
    // 1 : alkohol
    // 2 : cloth
    // 3 : kabel
    // 4 : besi
    // 5 : botol
    GameObject pickedUp;

    void Awake()
    {
        if (rb == null)
        {
            playerObj = GameObject.Find("PF Player");
            rb = playerObj.GetComponent<Rigidbody2D>();
            animator = playerObj.GetComponent<Animator>();

            playerLight = GameObject.Find("Player Direction light");
            playerLight.transform.rotation = Quaternion.Euler(0, 0, 0);
            lightPlayer = playerLight.GetComponent<Light2D>();
            lightPlayer.intensity = 1;

            range = 6;
            rawItems = new int[6] { 1, 2, 2, 3, 5, 7 };
            items = new int[3] { 3, 3, 3};
            pickedUp = null;
            fstate = false;
            maxBackpack = 30;
            flagc = true;
        }
        look = 4;
    }

    private void Update()
    {
        processInput();
    }

    void FixedUpdate()
    {
        updateOrientationPlayer();
        //walkAnim();
        //spriteOrientation();
    }

    void processInput()
    {
        // movement keybind
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }
        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        // item keybind
        if (Input.GetKey(KeyCode.Alpha1))
        {
            idxItem = 0;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            idxItem = 1;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            idxItem = 2;
        }
        if (Input.GetKey(KeyCode.F))
        {
            // turn on flashlight
            if (lightPlayer.intensity >= 0)
            {
                fstate = true;
            }
            else
            {
                fstate = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            flagc = true;
        }
        if (Input.GetKey(KeyCode.C) && flagc)
        {
            // Ke UI craft
            // setiap item yang dicraft akan mengurangi raw item dan menambah 1 item 
            flagc = false;
            if (inventory.active)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
                GameObject[] rawItems = new GameObject[6]
                {
                    GameObject.Find("Text Battery"),
                    GameObject.Find("Text Alkohol"),
                    GameObject.Find("Text Cloth"),
                    GameObject.Find("Text Wire"),
                    GameObject.Find("Text Iron"),
                    GameObject.Find("Text Bottle")
                };

                for (int i = 0; i < 6; i++)
                {
                    rawItems[i].GetComponent<Text>().text = this.rawItems[i] + "";
                }
            }
        }

        // kurangi flashlight life 
        if (fstate)
        {
            lightPlayer.intensity -= (float)0.00001;
            if(lightPlayer.intensity <= 0)
            {
                fstate = false;
            }
        }

        // pickup item
        if (Input.GetKey(KeyCode.E))
        {
            if (countBackpack() + 1 <= maxBackpack)
            {
                // cek collider dia ambil item apa
                if (pickedUp != null)
                {
                    string namaItem = pickedUp.name;
                    int idx = -1;
                    if (namaItem.Contains("Batrei"))
                    {
                        idx = 0;
                    }
                    else if(namaItem.Contains("Alkohol"))
                    {
                        idx = 1;
                    }
                    else if (namaItem.Contains("Cloth"))
                    {
                        idx = 2;
                    }
                    else if (namaItem.Contains("Kabel"))
                    {
                        idx = 3;
                    }
                    else if (namaItem.Contains("Besi"))
                    {
                        idx = 4;
                    }
                    else if (namaItem.Contains("Botol"))
                    {
                        idx = 5;
                    }
                    Debug.Log(namaItem);
                    rawItems[idx]++;
                    Destroy(pickedUp);
                    pickedUp = null;
                }
            }
        }

        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.KeypadEnter))
        {
            // 0 : burning cloth
            // 1 : bottle
            // 2 : Health
            if(items[idxItem] - 1 >= 0)
            {
                items[idxItem]--;
                Instantiate(PFitem[idxItem]);
            }
            else
            {
                Debug.Log("Don't have any");
            }
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        rb.velocity = speed * dir;
    }

    public int countBackpack()
    {
        int total = 0;
        // raw items take 80% backpack
        for (int i = 0; i < 6; i++)
        {
            total += rawItems[i];
        }
        total -= (int)(total * 0.2);

        // items take 100% backpack
        for (int i = 0; i < 3; i++)
        {
            total += items[i];
        }
        return total;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rawItem")
        {
            pickedUp = collision.gameObject;
            Debug.Log(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pickedUp = null;
    }

    public void updateOrientationPlayer()
    {
        Vector3 targetPosition = UtilsClass.GetWorldPositionFromUI();
        curAngle = UtilsClass.GetAngleFromVectorFloat((targetPosition - transform.position).normalized);
        playerLight.transform.rotation = Quaternion.Euler(0, 0, curAngle - 90);
    }

    /*void spriteOrientation()
    {
        if (fov.curAngle > 60 && fov.curAngle < 170) look = 1;
        else if (fov.curAngle > 170 && fov.curAngle < 250) look = 3;
        else if (fov.curAngle > 250 && fov.curAngle < 330) look = 4;
        else if (fov.curAngle < 60 || fov.curAngle > 330) look = 2;

        animator.SetInteger("Look", look);
        look = 0;
    }

    void walkAnim()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x + rb.velocity.y));
        if (Mathf.Abs(rb.velocity.x + rb.velocity.y) > 2) animator.speed = 1.3f;
        else animator.speed = 0.6f;
    }*/
}
