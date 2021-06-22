using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Steering {
    public Vector2 linear { get; set; }
    public float angular { get; set; }
}

public class EnemyController : MonoBehaviour
{
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

    [Header("NanoBot")]
    [SerializeField] private GameObject nanoBotPrefab;
    [SerializeField] private bool spawnNanobot;

    [SerializeField] private int nanoBotDelay; // brp ctr sblm spawn nanobot
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
    private bool targetIsWaypoint = false;
    private float delayTarget = 0;
    private Vector2 target;
    // NanoBot
    private int ctrNano = 0;
    // Glitch Bot
    private bool isFreeze = false;

    public EnemyController() {
        id = ctr_id++;
    }

    // Update is called once per frame
    void FixedUpdate() {
        isFreeze = false;
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
        // Ctr Nanobot 
        if (spawnNanobot) {
            ctrNano++;
            if(ctrNano >= nanoBotDelay) {
                ctrNano = 0;
                // Clone Nano bot di posisi Induk
                var nanobot = nanoBotPrefab;
                var tempGrapCtrl = nanobot.GetComponent<EnemyController>();
                tempGrapCtrl.graphWaypoint = this.graphWaypoint;
                var newBot = GameObject.Instantiate(nanobot);
                newBot.transform.position = this.rb.transform.position;
            }
        }
    }

    // on collide
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("player")) {
            if (isExplode) { // Jika explore maka meledak
                Debug.Log($"{this.gameObject.name} explode!");
                Destroy(this.gameObject);
            }
        }else if (col.gameObject.CompareTag("wall")) {
            // harusny ga mungkin
            // tapi jika mungkin maka cara waypoint terdekat
            Debug.Log("Nabrak wall");
            this.setTargetToNearestWaypoint();
        }
    }

    // Helper Function
    void getTarget() {
        // Check Player
        var player = getPlayerAround();
        rb.drag = 0;
        if (player != null) { // Jika melihat player maka target jadi player
            Debug.Log("See player!");
            if (isWheepingAngel) {
                isFreeze = true;
                rb.drag = 5;
            } else {
                this.target = player.transform.position;
                this.targetIsWaypoint = false;
                this.hasTarget = true;
            }
        } else { // Pergi ke Target
            Vector2 origin = this.gameObject.transform.position;
            if(delayTarget < delayLostPlayer) {
                delayTarget += Time.deltaTime;
            }else if (!this.hasTarget) { // Jika tidak punya target
                setTargetToNearestWaypoint();
            } else {
                bool isArrive = Vector2.Distance(origin, target) < 1; // Check sudah sampai posisi
                if (isArrive) { // Jika sudah sampai
                    Debug.Log("Reset target");
                    this.hasTarget = false;
                    if (this.targetIsWaypoint) { // Jika targetnya waypoint mk cari waypoint tetangga
                        this.hasTarget = true;
                        this.target = graphWaypoint.getRandomNeighbourPos(origin, isWallIgnore);
                    } else { // Jika targetnya player, mk cari waypoint terdekat
                        delayTarget = 0;
                    }
                }
            }
        }
    }

    void setTargetToNearestWaypoint() {
        Vector2 origin = this.gameObject.transform.position;
        this.targetIsWaypoint = true;
        this.hasTarget = true;
        this.target = graphWaypoint.getNearestWaypointPos(origin, isWallIgnore);
        Debug.Log("New Target: " + target);
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
                Debug.Log($"Hit {otherObj.tag}");
                if (otherObj.CompareTag("Player")) {
                    player = otherObj;
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
}
