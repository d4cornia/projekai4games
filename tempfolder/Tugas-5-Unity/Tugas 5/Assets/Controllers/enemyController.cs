using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Steering
{
    public Vector2 linear { get; set; }
    public float angular { get; set; }
}

public class enemyController : MonoBehaviour
{
    public static void initEnemies() {
        GameObject enemyObject = GameObject.Find("Enemy");
        int numberOfEnemy = 5;
        for (int i = 0; i < numberOfEnemy; i++) {
            GameObject newObj = Instantiate(enemyObject);
            Waypoint waypoint = waypointController.graph.getRandomWaypoint();
            newObj.transform.position = waypoint.position;
            newObj.GetComponent<enemyController>().lastWaypoint = waypoint;
        }
        Destroy(enemyObject);
    }

    public Rigidbody2D rb;
    public Rigidbody2D rbTarget;
    public GameObject enemyObj;
    public float maxSpeed;
    public float acceleration;

    public int radiusPlayerDetection;
    public int coneEvadeRadius;

    public fieldOfView fov;
    public fieldOfView blindSpotFOV;

    private static int ctr_id = 0;
    private int id;

    // Waypoint AI skrg
    private float targetRadius = 0;
    private Waypoint targetWaypoint;
    private Waypoint lastWaypoint;

    public enemyController() {
        id = ctr_id++;
        // Init Target Lokasi
        this.targetWaypoint = null;
        this.lastWaypoint = null;
    }

    // Start is called before the first frame update
    void Awake(){
        if (rb == null){
            rb = enemyObj.GetComponent<Rigidbody2D>();
            rbTarget = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            fov = Instantiate(GameObject.Find("fov").GetComponent<fieldOfView>());
            fov.name = "enemyFOV" + enemyObj.name;
            fov.fov = 140;
            fov.viewDistance = 5;

            blindSpotFOV = Instantiate(GameObject.Find("fov").GetComponent<fieldOfView>());
            blindSpotFOV.name = "enemyblindSpotFOV" + enemyObj.name;
            blindSpotFOV.fov = 400;
            blindSpotFOV.viewDistance = 2;
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        // Check apakah sudah saatnya pindah waypoint
        // & Pergi ke waypoint jika ada target waypoint
        checkPosition();
        // Check apakah sudah sampai waypoint
        checkReachWaypoint();
        // Check kecepatan
        if (rb.velocity.magnitude > maxSpeed){
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    // Helper Function
    GameObject getPlayerAround() {

        GameObject player = null;
        float x = this.rb.velocity.x;
        float y = this.rb.velocity.y;
        float degreeVelo = Mathf.Atan(x / y);
        //for (int deg = -coneEvadeRadius / 2; deg < coneEvadeRadius / 2; deg++) {
        //    float degree = degreeVelo + deg;
        //}
        for (int deg = 0; deg < 360; deg++) {
            float degree = deg;
            Vector2 direction = new Vector2(Mathf.Sin(degree), Mathf.Cos(degree));
            Vector3 origin = (Vector3)this.rb.position + (Vector3)direction;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, this.radiusPlayerDetection);
            // layer mask -> yang mau dikenain layer apa aja, bukan di ignore
            if (raycastHit2D.collider != null) {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Player")) {
                    player = otherObj;
                }
            }
        }
        return player;
    }

    void moveToWaypoint(Waypoint targetWaypoint) {
        this.targetWaypoint = targetWaypoint;
        this.targetRadius = Random.Range(0.5f, 1);
        this.rb.drag = 0;
        Debug.Log($"{this.name} start running! to ${this.targetWaypoint.name}");
    }

    void checkPosition(){
    	// Buat check waypiint AI
    	if(this.targetWaypoint == null){
            //Debug.Log("New Target!");
            // calculate cover value last Waypoint
            float coverValue = this.lastWaypoint.coverVal;
            // Jika Cover value rendah pindah Waypoint tetangga dengan Cover value tertinggi
            if (coverValue < 6) { // Posisi tidak aman
                Waypoint targetWaypoint = this.lastWaypoint.getHighestCoverNeighbour();
                moveToWaypoint(targetWaypoint);
            } else { 
                // Posisi aman
            }
        } else { // Jika ada waypoint gerak
            // Move
            Steering steering = move_seek(this.targetWaypoint.position);
            updateMovement(steering);
            GameObject player = getPlayerAround();
            if(player != null) {
                Vector2 position = this.rb.position;
                Waypoint targetWaypoint = waypointController.graph.getClosestWaypoint(position);
                moveToWaypoint(targetWaypoint);
            }
        }
    } 

    void checkReachWaypoint() {
        if(targetWaypoint == null) {
            return;
        }
        Vector2 position = this.transform.position;
        Vector2 targetPosition = this.targetWaypoint.position;
        Vector2 diffPosition = position - targetPosition;
        // Jika sudah smp tujuan (Waypoint)
        if(diffPosition.magnitude < this.targetRadius) {
            this.lastWaypoint = this.targetWaypoint;
            this.targetWaypoint = null;
            this.rb.AddForce(Vector2.zero, ForceMode2D.Force);
            this.rb.drag = 25;
            Debug.Log($"{this.name} reached waypoint!");
        }
    }

    // AI MOVEMENT
    void updateMovement(Steering steering){
        if (steering == null){
            rb.AddForce(new Vector2(), ForceMode2D.Impulse);
        }else{
            rb.AddForce(steering.linear * Time.deltaTime, ForceMode2D.Impulse);
            fov.curAngle = UtilsClass.GetAngleFromVector(rb.velocity.normalized) + fov.fov / 2f;
            fov.setOrigin(rb.transform.position);
            blindSpotFOV.setOrigin(rb.transform.position);
        }
    }

    Steering move_seek() => move_seek(rbTarget.position);
    Steering move_seek(Vector2 target){
        Steering steering = new Steering();
        steering.linear = target - rb.position;
        steering.linear = steering.linear.normalized * acceleration;
        steering.angular = 0;
        return steering;
    }
}
