using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

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
    public int coin;
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
    // 6 : duct tape
    GameObject pickedUp;


    // keys
    public List<string> keys;

    public GameObject[] activeItem;
    public GameObject[] ctrTexts;

    // objectives
    public List<string> listObj;


    //health
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;


    [Header("Slow Player")]
    public float dragSlow;
    public int timeSlow;

    // Drag Slow
    int ctrSlow = 0;

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
            rawItems = new int[7] { 1, 2, 2, 3, 5, 7, 3 };
            items = new int[3] { 3, 3, 3};
            pickedUp = null;
            fstate = true;
            maxBackpack = 30;
            flashLife = MAX_FLASH;
            coin = 250;

            flagi = false;
            flagM = false;

            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            listObj = new List<string>();
            updateCtrItem();
        }
        look = 4;
    }

    private void Update()
    {
        if (!endGame())
        {
            processInput();
        }
    }

    // Function yang dipanggil ketika enemy hit Player
    public void TakeDamage(int damage, GameObject hitter){
        Debug.Log("Player Take Damage!");
        //
        Vector2 diffPos = hitter.transform.position - this.gameObject.transform.position;
        Vector2 direction = -diffPos.normalized;
        this.rb.AddForce(direction * 3000);
        //
        if(currentHealth - damage >= 0){
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }else{
            currentHealth = 0;
        }
        Debug.Log($"Player HP: {currentHealth}!");
        // TODO Knockback(?)
    }

    void FixedUpdate()
    {
        if (!endGame())
        {
            updateOrientationPlayer();
            if (fstate)
                counterWheepingAngle();
        }
        walkAnim();
        spriteOrientation();
    }

    public bool endGame()
    {
        if(health <= 0)
        {
            // game endded
            return true;
        }
        return false;
    }

    public void counterWheepingAngle()
    {
        for (float deg = (curAngle - fov / 2); deg < (curAngle + fov / 2); deg++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * deg), Mathf.Sin(Mathf.Deg2Rad * deg));
            Vector3 offset = new Vector3((float)(Mathf.Cos(Mathf.Deg2Rad * deg) * 0.50), (float)(Mathf.Sin(Mathf.Deg2Rad * deg) * 0.50), 0);
            Vector3 origin = (Vector3)this.rb.position + offset;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, (float)(range - 1));
            if (raycastHit2D.collider != null)
            {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("enemy"))
                {
                    EnemyController enemyCon = otherObj.GetComponent<EnemyController>();
                    if (enemyCon.isWheepingAngel)
                    {
                        if (enemyCon.freezeTime == 0)
                        {
                            enemyCon.freezeTime = 5;
                            StartCoroutine(enemyCon.CountDown());
                        }
                    }
                }
            }
        }
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
                        else if (namaItem.Contains("Duct Tape"))
                        {
                            idx = 6;
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
                    // show text dapat keynya
                    pickedUp.GetComponent<objectiveController>().reqTextGO.GetComponent<reqTextController>().showText();
                    pickedUp.GetComponent<objectiveController>().reqTextGO.GetComponent<TextMeshProUGUI>().text = $"Got {pickedUp.name}";

                    keys.Add(pickedUp.name);
                    destroyItem();
                    pickedUp = null;
                }
                else if (pickedUp.tag == "Chest")
                {
                    if (checkPickedupReqObj())
                    {
                        pickedUp.GetComponent<keySpawner>().destroyChest();
                    }
                }
                else if (pickedUp.tag == "Door")
                {
                    if (checkPickedupReqObj())
                    {
                        // script door ambil next location dan tp kesana
                        pickedUp.GetComponent<doorController>().tp();
                    }
                }
                else if (pickedUp.tag == "Interactable")
                {
                    pickedUp.GetComponent<objectiveController>().showTextReq();
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

    public bool checkPickedupReqObj()
    {
        if (pickedUp.GetComponent<objectiveController>().requirement())
        {
            return true;
        }
        pickedUp.GetComponent<objectiveController>().showTextReq();
        pickedUp.GetComponent<objectiveController>().finishAndNewObjective();
        pickedUp = null;
        return false;
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
        if (listObj.Count == 0) objectiveText.GetComponent<TextMeshProUGUI>().text = "None";
        else
        {
            objectiveText.GetComponent<TextMeshProUGUI>().text = "";
            foreach (var item in listObj)
            {
                objectiveText.GetComponent<TextMeshProUGUI>().text += "- " + item + "\n";
            }
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
            ctrTexts[3].GetComponent<TextMeshProUGUI>().text = rawItems[0] + "";
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
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, 0.25f);
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
            ctrTexts[i].GetComponent<TextMeshProUGUI>().text = items[i] + "";
        }

        ctrTexts[3].GetComponent<TextMeshProUGUI>().text = rawItems[0] + "";
    }

    public int countBackpack()
    {
        int total = 0;
        // raw items take 80% backpack
        for (int i = 0; i < 7; i++)
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
        Debug.Log(collision.tag);
        if (collision.tag == "rawItem" || collision.tag == "Item" || collision.tag == "Key" || collision.tag == "Chest" || collision.tag == "Door")
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

    // Slow Player
    public void slowPlayer() {
        this.rb.drag = dragSlow;
        this.ctrSlow = timeSlow;
        if (this.ctrSlow > 0) {
            StartCoroutine(CountDownSlow());
        }
    }

    public IEnumerator CountDownSlow() {
        while (ctrSlow > 0) {
            yield return new WaitForSeconds(1f);
            ctrSlow--;
        }
        this.rb.drag = 0;
    }


    void spriteOrientation()
    {
        if (curAngle > 35 && curAngle < 150) look = 1;
        else if (curAngle > 150 && curAngle < 230) look = 3;
        else if (curAngle > 230 && curAngle < 310) look = 4;
        else if (curAngle < 35 || curAngle > 310) look = 2;
        Debug.Log("Angle : " + curAngle + " | Looking : " + look);
        animator.SetInteger("Look", look);
        look = 0;
    }

    void walkAnim()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x + rb.velocity.y));
    }
}
