using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Steering {
    public Vector2 linear { get; set; }
    public float angular { get; set; }
}

enum TargetType {
    NONE,
    WAYPOINT,
    PLAYER,
    DECOY
}

public class EnemyController : MonoBehaviour {
    //// Parameters
    [Header("General")]
    [SerializeField] private GraphWaypointController graphWaypoint;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float delayLostPlayer;
    [Range(0,360)][SerializeField] private int coneDegree;
    [SerializeField] private float coneRadius;
    [SerializeField] private int damage;
    [SerializeField] private int INCDEG = 1;

    [Header("Spawner Bot")]
    [SerializeField] private GameObject nanoBotPrefab;
    [SerializeField] private bool spawnNanobot;
    [SerializeField] private int nanoBotsPerSpawn; // brp ctr sblm spawn nanobot
    [SerializeField] private int spawnNanoBotDelay; // brp ctr sblm spawn nanobot

    [Header("Nano Bot")]
    [SerializeField] private bool isNanobot;
    [SerializeField] private float lifeSpan; // umur nanobot dlm detik

    [Header("Glitch Bot")]
    [SerializeField] private bool isWheepingAngel;

    [Header("Kamikaze")]
    [SerializeField] private bool isExplode;

    [Header("Warper")]
    [SerializeField] private bool isWallIgnore;  
    //// End of Parameters
    
    // Id
    private static int ctr_id = 0;
    private int id;
    // Target
    private bool hasTarget = false;
    private TargetType targetType = TargetType.NONE;
    private float delayTarget = 0;
    private Vector2 target; // Target: waypoint, player, decoy, burning cloth
    private GameObject decoyTarget = null; // Diisi ketika punya decoy target
    // Spawner
    private List<GameObject> childs = new List<GameObject>();
    private float delaySpawn = 0;
    // NanoBot
    private GameObject parentNanobot = null;
    private float life = 0; // umur nano bot

    // Glitch Bot
    private bool isFreeze = false;

    public EnemyController() {
        id = ctr_id++;
    }

    // Update is called once per frame
    void FixedUpdate() {
        preUpdate();
        // Get Target
        getTarget();
        if (isFreeze) return;
        // Jalan ke Target
        if (this.hasTarget) {
            Steering steering = move_seek(this.target);
            updateMovement(steering);
        }
        // Check kecepatan
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        postUpdate();
    }

    // on collide
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            // Damage Player
            var player = col.gameObject;
            var controller = player.GetComponent<playerController>();
            controller.TakeDamage(this.damage, this.gameObject);
            Debug.Log($"Hit Player! {this.damage}");
            // Jika Kamikaze
            if (isExplode) { // Jika explore maka meledak
                Debug.Log($"{this.gameObject.name} explode!");
                Destroy(this.gameObject);
            }
        }
        //else if (col.gameObject.CompareTag("wall")) {
        //    // harusny ga mungkin
        //    // tapi jika mungkin maka cara waypoint terdekat
        //    Debug.Log("Nabrak wall");
        //    this.setTargetToNearestWaypoint();
        //}
    }

    void preUpdate() { // Function yg dipanggil tiap kali update belum dilakukan (dibuat nambah ctr dll)
        // Wheeping Angel
        isFreeze = false; 
        rb.drag = 0;

    }
    void postUpdate() { // Function yg dipanggil tiap kali update selesai dilakukan (dibuat nambah ctr dll)
        // Spawner
        if (spawnNanobot) {
            if (delaySpawn > 0) {
                delaySpawn -= Time.deltaTime;
            }
        }
        // NanoBot
        if (isNanobot) {
            life += Time.deltaTime;
            if (life >= lifeSpan) { // Jika sudah melebihi batas hidup
                var controller = parentNanobot.GetComponent<EnemyController>();
                controller.childs.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    // Helper Function
    void getTarget() {
        if (decoyTarget != null) { // Jika target adalah decoy
            this.target = decoyTarget.transform.position; // Update Target Position
            return;
        }else if (targetType == TargetType.DECOY) { // Jika tidak ada decoy tapi 
            setTargetToNearestWaypoint();
            return;
        }

        // Check Burning Cloth & Decoy Around
        var items = GameObject.FindGameObjectsWithTag("Item");
        foreach (var item in items) {
            Vector2 itemPos = item.transform.position;
            Vector2 vector = itemPos - (Vector2)this.transform.position;
            var btlController = item.GetComponent<BottleController>();
            var brnController = item.GetComponent<BurningClothController>();
            if(btlController != null || brnController != null) { // Jika null dua" maka buang
                float radius = btlController == null ? brnController.range : btlController.range;
                Debug.Log($"{item}, {vector.magnitude}-{radius}");
                if (vector.magnitude < radius) { // Ketemu Decoy
                    reactToTarget(item, TargetType.DECOY);
                    return;
                }
            }
        }

        // Check Found Player
        GameObject player = getPlayerAround();
        if (player != null) { // Jika melihat player maka target jadi player
            reactToTarget(player, TargetType.PLAYER);
        } else { // State Pergi ke Target...
            Vector2 origin = this.gameObject.transform.position;
            if(delayTarget < delayLostPlayer) { // Check apakah barusan kehilangan player
                delayTarget += Time.deltaTime;
            }else if (!this.hasTarget) { // Jika tidak punya target
                setTargetToNearestWaypoint();
            } else {
                bool isArrive = Vector2.Distance(origin, target) < 1; // Check sudah sampai posisi
                if (isArrive) { // Jika sudah sampai
                    Debug.Log("Reset target");
                    this.hasTarget = false;
                    if (this.targetType == TargetType.WAYPOINT) { // Jika targetnya waypoint mk cari waypoint tetangga
                        setTargetToNeighbourWaypoint();
                    } else if(this.targetType == TargetType.PLAYER){ // Jika targetnya player, mk cari waypoint terdekat
                        delayTarget = 0;
                    }
                }
            }
        }
    }

    void reactToTarget(GameObject gameObject, TargetType targetType) {
        if(targetType == TargetType.DECOY) {
            decoyTarget = gameObject;
        }

        if (spawnNanobot) { // Jika spawner
            tryGenerateNanobot();
            informNanobots(gameObject, targetType);
        } else if (isNanobot) { // Jika Nanobot
            var controllerParent = parentNanobot.GetComponent<EnemyController>();
            controllerParent.informNanobots(gameObject, targetType);
        } else if (isWheepingAngel) {// Jika Wheeping Angel
            isFreeze = true;
            rb.drag = 5;
        } else { // Jika Selain bot diatas
            setTargetToGameObject(gameObject, targetType);
        }
    }

    void setTargetToGameObject(GameObject targetObject, TargetType targetType) {
        this.hasTarget = true;
        this.target = targetObject.transform.position;
        this.targetType = targetType;
        if (targetType == TargetType.DECOY) {
            this.decoyTarget = targetObject;
        }
    }

    void setTargetToNearestWaypoint() {
        Vector2 origin = this.gameObject.transform.position;
        this.hasTarget = true;
        this.target = graphWaypoint.getNearestWaypointPos(origin, isWallIgnore);
        this.targetType = TargetType.WAYPOINT;
    }

    void setTargetToNeighbourWaypoint() {
        Vector2 origin = this.gameObject.transform.position;
        this.hasTarget = true;
        this.target = graphWaypoint.getRandomNeighbourPos(origin, isWallIgnore);
        this.targetType = TargetType.WAYPOINT;
    }

    GameObject getPlayerAround() { // Dapetin Player
        GameObject player = null;
        int sign = this.rb.velocity.x > 0 ? 1 : -1; // Dapetin Polaritas
        float degreeVelo = Vector2.Angle(new Vector2(0,1), this.rb.velocity); // Dapetin Degreenya arah hadepannya
        degreeVelo *= sign; // Dapetin Degree + polaritas
        for (int deg = -coneDegree/2; deg < coneDegree/2; deg += INCDEG) {
            float degree = (degreeVelo + deg) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(degree), Mathf.Cos(degree));
            Vector3 origin;
            //origin = (Vector3)this.rb.position + (Vector3)direction;
            origin = (Vector3)this.rb.position;
            RaycastHit2D raycastHit2D;
            if (!isWallIgnore) { // Jika tidak wall ignore maka cast biasa
                int layerMask = 1 << LayerMask.NameToLayer("Layer 2");
                raycastHit2D = Physics2D.Raycast(origin, direction, coneRadius, layerMask);
            } else { // Jika wall ignore maka cast hanya layer player
                int layerMask = 1 << LayerMask.NameToLayer("Layer 2");
                raycastHit2D = Physics2D.Raycast(origin, direction, coneRadius, layerMask);
            }
            Debug.DrawRay(origin, direction.normalized * coneRadius, Color.red);
            if (raycastHit2D.collider != null) { // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Player")) {
                    player = otherObj;
                    break;
                }
            }
        }
        return player;
    }

    // AI MOVEMENT
    Steering move_seek(Vector2 target) {
        Steering steering = new Steering();   
        steering.linear = target - rb.position;
        steering.linear = steering.linear.normalized * acceleration;
        steering.angular = 0;
        return steering;
    }

    void updateMovement(Steering steering) {
        if (steering == null) {
            rb.AddForce(new Vector2(), ForceMode2D.Impulse);
        } else {
            rb.AddForce(steering.linear * Time.deltaTime);
        }
    }

    // Spawner
    void tryGenerateNanobot() {
        if (delaySpawn <= 0) {// Clone nanobot
            // Prefab Setup
            var nanobot = nanoBotPrefab;
            var tempController = nanobot.GetComponent<EnemyController>();
            tempController.graphWaypoint = this.graphWaypoint;
            for (int i = 0; i < nanoBotsPerSpawn; i++) {
                // Clone
                var newBot = GameObject.Instantiate(nanobot);
                newBot.transform.position = this.rb.transform.position;
                tempController = newBot.GetComponent<EnemyController>();
                tempController.parentNanobot = this.gameObject;
                // Simpan d List
                childs.Add(newBot);
            }
            // Update Delay
            delaySpawn = spawnNanoBotDelay;
        }
    }

    void informNanobots(GameObject gameObject, TargetType targetType) {
        foreach (var nanobot in childs) {
            var controller = nanobot.GetComponent<EnemyController>();
            controller.setTargetToGameObject(gameObject, targetType);
        }
        this.setTargetToGameObject(gameObject, targetType);
    }
}
