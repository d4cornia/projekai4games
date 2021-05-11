using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphWaypoint {
    public List<Waypoint> waypoints { get; set; }

    public Waypoint getClosestWaypoint(Vector2 location) {
        return waypoints[0];
    }
    public Waypoint getRandomWaypoint() {
        int idx = Random.Range(0, this.waypoints.Count);
        return this.waypoints[idx];
    }
}

public class Waypoint {
    // Posisi Waypoint
    public Vector2 position { get; set; }
    // Attributes Waypoint
    public bool isCover { get; set; }
    public bool isShadow { get; set; }
    // References
    public List<Waypoint> waypoints { get; set; }

    // Constructor
    public Waypoint(Vector2 position, bool isCover, bool isShadow) {
        this.position = position;
        this.isCover = isCover;
        this.isShadow = isShadow;
        this.waypoints = new List<Waypoint>();
    }

    // Method
    public Waypoint getRandomNeighbour() {
        int idx = Random.Range(0, this.waypoints.Count);
        return this.waypoints[idx];
    }
}

public class waypointController : MonoBehaviour{
    static int ctrWaypoint = 0;
    public static GraphWaypoint graph = null;
    static void incrementWaypoint() {
        ctrWaypoint++;
        var gameObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        int totalWaypoint = gameObjects.Length;
        if (ctrWaypoint == totalWaypoint) {
            // Create Graph
            List<Waypoint> listWaypoint = new List<Waypoint>();
            foreach (var gameObject in gameObjects) {
                waypointController controller = gameObject.GetComponent<waypointController>();
                Waypoint waypoint = controller.waypoint;
                foreach (var waypointController in controller.waypointControllers) {
                    waypoint.waypoints.Add(waypointController.waypoint);
                }
                listWaypoint.Add(waypoint);
            }
            GraphWaypoint graph = new GraphWaypoint { waypoints = listWaypoint };
            waypointController.graph = graph;
            Debug.Log(listWaypoint.Count);
            // Remove GameObject
            foreach (var gameObject1 in gameObjects) {
                gameObject1.GetComponent<SpriteRenderer>().sprite = null;
                gameObject1.GetComponent<CircleCollider2D>().enabled = false;
            }
            // Spawn Enemy
            enemyController.initEnemies();
        }
    }

    public GameObject obj;
    public List<waypointController> waypointControllers;
    public Waypoint waypoint;
    public float coverValue;

    private void Awake() {
        waypointControllers = new List<waypointController>();
        waypoint = new Waypoint(
            this.GetComponent<Transform>().position,
            false,
            false
        );
    }

    private void Start() {
        check360();
    }

    void check360() {
        // Check 360
        for (int deg = 0; deg < 360; deg++) {
            Vector2 direction = new Vector2(Mathf.Sin(deg), Mathf.Cos(deg));
            Vector3 origin = obj.GetComponent<Transform>().position + (Vector3)direction;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction);
            if (raycastHit2D.collider == null) {
                // no hit
            } else {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Waypoint")) {
                    waypointController objController = otherObj.GetComponent<waypointController>();
                    if (!waypointControllers.Contains(objController)) {
                        this.waypointControllers.Add(objController);
                        objController.waypointControllers.Add(this);
                    }
                }
            }
        }
        string str = "";
        foreach (var waypointController in waypointControllers) {
            str += waypointController.name + ", ";
        }
        Debug.Log($"{this.name}\n{str}");
        waypointController.incrementWaypoint();
    }
}
