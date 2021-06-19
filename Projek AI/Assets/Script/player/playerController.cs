using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class playerController : MonoBehaviour
{
    // base
    public Rigidbody2D rb;
    public GameObject playerLight;
    public GameObject playerObj;
    public GameObject objective;
    public GameObject objectiveText;
    public Inventory inventory;
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
    public int money;
    public const float MAX_FLASH = .5f;


    // flashlight
    Light2D lightPlayer;
    bool fstate;
    float flashLife;


    // prefab item
    public GameObject[] PFitem;


    //item
    bool flagi, flagM;
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


    // keys
    public List<string> keys;

    public GameObject[] activeItem;
    public GameObject[] texts;

    // objectives
    public List<string> listObj;


    //health
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Awake()
    {
        if (rb == null)
        { 
            playerObj = GameObject.Find("PF Player");
            rb = playerObj.GetComponent<Rigidbody2D>();
            animator = playerObj.GetComponent<Animator>();

            inventory = playerObj.GetComponent<Inventory>();

            playerLight.transform.rotation = Quaternion.Euler(0, 0, 0);
            lightPlayer = playerLight.GetComponent<Light2D>();
            lightPlayer.intensity = 1;

            health = 100;
            range = 6;
            rawItems = new int[6] { 1, 2, 2, 3, 5, 7 };
            items = new int[3] { 3, 3, 3};
            pickedUp = null;
            fstate = true;
            maxBackpack = 30;
            flashLife = MAX_FLASH;
            money = 250;

            flagi = false;
            flagM = false;

            listObj = new List<string>();
            listObj.Add("Get Blue Key");
            updateCtrItem();
        }
        look = 4;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
        }*/
        processInput();
    }

    void TakeDamage(int damage)
    {
        if(currentHealth - damage >= 0)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            currentHealth = 0;
        }
    }

    void FixedUpdate()
    {
        updateOrientationPlayer();
        //walkAnim();
        //spriteOrientation();
    }

    void processInput()
    {
        Vector2 dir = Vector2.zero;
        // jika tidak lagi open merchant
        if (!flagM)
        {
            // movement keybind
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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                idxItem = 0;
                flagi = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                idxItem = 1;
                flagi = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                idxItem = 2;
                flagi = true;
            }
            if (flagi)
            {
                flagi = false;
                for (int i = 0; i < 3; i++)
                {
                    if (i == idxItem)
                    {
                        activeItem[i].SetActive(true);
                    }
                    else
                    {
                        activeItem[i].SetActive(false);
                    }
                }
            }



            //flashlihght
            if (Input.GetKeyDown(KeyCode.F))
            {
                // turn off flashlight
                if (fstate)
                {
                    lightPlayer.intensity = 0;
                    fstate = false;
                }
                else
                {
                    lightPlayer.intensity = flashLife;
                    fstate = true;
                }
            }


            // Reload senter
            if (Input.GetKeyDown(KeyCode.R))
            {
                reloadBattery();
            }


            // Crafting item
            if (Input.GetKeyDown(KeyCode.C))
            {
                // Ke UI craft
                // setiap item yang dicraft akan mengurangi raw item dan menambah 1 item 
                inventory.openInventory();
            }


            // use item
            if (Input.GetKeyDown(KeyCode.G))
            {
                // 0 : burning cloth
                // 1 : bottle
                // 2 : Health
                if (items[idxItem] - 1 >= 0)
                {
                    items[idxItem]--;
                    if (idxItem != 2)
                    {
                        Instantiate(PFitem[idxItem]);
                    }
                    else
                    {
                        if (currentHealth + 30 <= maxHealth)
                        {
                            currentHealth += 30;
                        }
                        else
                        {
                            currentHealth = maxHealth;
                        }
                        healthBar.SetHealth(currentHealth);
                    }
                    updateCtrItem();
                }
                else
                {
                    Debug.Log("Don't have any");
                }
            }


            // view objective 
            if (Input.GetKeyDown(KeyCode.O))
            {
                objective.SetActive(!objective.active);
            }
        }


        // pickup item and interact with merchant
        if (Input.GetKeyDown(KeyCode.E))
        {
            // jika diarea merchant, open merchant
            checkMerchant();

            // cuman bisa pickup rawItem
            if (pickedUp != null)
            {
                if(pickedUp.tag == "rawItem")
                {
                    if (countBackpack() + 0.8 <= maxBackpack)
                    {
                        // cek collider dia ambil item apa
                        string namaItem = pickedUp.name;
                        int idx = -1;
                        if (namaItem.Contains("Batrei"))
                        {
                            idx = 0;
                        }
                        else if (namaItem.Contains("Alkohol"))
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
                        destroyItem();
                        pickedUp = null;
                        updateCtrItem();
                    }
                    else
                    {
                        Debug.Log("Your Backpack is full");
                    }
                }
                else if(pickedUp.tag == "Item")
                {
                    if (countBackpack() + 1 <= maxBackpack)
                    {
                        // cek collider dia ambil item apa
                        string namaItem = pickedUp.name;
                        int idx = -1;
                        if (namaItem.Contains("Burning Cloth"))
                        {
                            idx = 0;
                        }
                        else if (namaItem.Contains("Decoy Bottle"))
                        {
                            idx = 1;
                        }
                        else if (namaItem.Contains("Bandage"))
                        {
                            idx = 2;
                        }
                        Debug.Log(namaItem);
                        items[idx]++;
                        destroyItem();
                        pickedUp = null;
                        updateCtrItem();
                    }
                    else
                    {
                        Debug.Log("Your Backpack is full");
                    }
                }
                else if(pickedUp.tag == "Key")
                {
                    if (pickedUp.GetComponent<objectiveController>().reqiurement())
                    {
                        keys.Add(pickedUp.name);
                        destroyItem();
                        pickedUp = null;
                    }
                }
                else if (pickedUp.tag == "Chest")
                {
                    pickedUp.GetComponent<keySpawner>().destroyChest();
                    pickedUp = null;
                }
            }
        }


        // kurangi flashlight life 
        if (fstate)
        {
            flashLife -= (float)0.00001;
            lightPlayer.intensity = flashLife;
            if (flashLife <= 0)
            {
                lightPlayer.intensity = 0;
            }
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        rb.velocity = speed * dir;
    }

    public void destroyItem()
    {
        if (pickedUp.GetComponent<objectiveController>())
        {
            pickedUp.GetComponent<objectiveController>().finishAndNewObjective();
        }
        Destroy(pickedUp);
    }

    public void updateObjective()
    {
        foreach (var item in listObj)
        {
            objectiveText.GetComponent<Text>().text = item + "\n ";
        }
    }

    public void changeFlashlight()
    {
        lightPlayer = playerLight.GetComponent<Light2D>();
    }

    public void reloadBattery()
    {
        if (rawItems[0] - 1 >= 0)
        {
            rawItems[0]--;
            flashLife = MAX_FLASH;
            texts[6].GetComponent<Text>().text = rawItems[0] + "";
        }
        else
        {
            Debug.Log("Don't have any battery left");
        }
    }

    public void checkMerchant()
    {
        bool flag = true;
        for (float deg = 0; deg < 360; deg++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
            Vector3 offset = new Vector3((float)(Mathf.Cos(Mathf.Deg2Rad * deg) * 0.50), (float)(Mathf.Sin(Mathf.Deg2Rad * deg) * 0.50), 0);
            Vector3 origin = playerObj.transform.position + offset;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, 2);
            if (raycastHit2D.collider != null)
            {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Merchant") && flag)
                {
                    flag = false;
                    flagM = !flagM;
                    otherObj.GetComponent<merchantController>().openMerchant();
                    break;
                }
            }
        }
    }

    public void updateCtrItem()
    {
        for (int i = 0; i < 3; i++)
        {
            texts[i].GetComponent<Text>().text = items[i] + "";
            texts[i + 3].GetComponent<Text>().text = items[i] + "";
        }

        texts[6].GetComponent<Text>().text = rawItems[0] + "";
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
        if (collision.tag == "rawItem" || collision.tag == "Item" || collision.tag == "Key" || collision.tag == "Chest")
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
