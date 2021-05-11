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
    public string name;
    public LayerMask layerMask;
    public Vector2 position { get; set; }
    // Attributes Waypoint
    public float coverValue;
    // References
    public List<Waypoint> waypoints { get; set; }

    // Constructor
    public Waypoint(Vector2 position,string name, LayerMask layermask) {
        this.position = position;
        this.name = name;
        this.waypoints = new List<Waypoint>();
        this.layerMask = layermask;
        Debug.Log("Layer mask : " + layerMask.value);
    }

    // Method
    public Waypoint getRandomNeighbour() {
        int idx = Random.Range(0, this.waypoints.Count);
        return this.waypoints[idx];
    }

    public void coverValueCalc()
    {
        for (int deg = 0; deg < 360; deg++)
        {
            Vector2 direction = new Vector2(Mathf.Sin(deg), Mathf.Cos(deg));
            Vector3 origin = (Vector3)position + (Vector3)direction;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, 250, layerMask);
            // layer mask -> yang mau dikenain layer apa aja, bukan di ignore
            if (raycastHit2D.collider != null)
            {
                // hit object
                GameObject otherObj = raycastHit2D.collider.gameObject;
                if (otherObj.CompareTag("Player"))
                {
                    coverValue = raycastHit2D.distance;
                }
            }
        }
        //Debug.Log("Nama : " + this.name + "Cover value : " + coverValue);
    }
}

public class waypointController : MonoBehaviour{
    public static int ctrWaypoint = 0;
    public static GraphWaypoint graph = null;
    public LayerMask layerMask;
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
            // Spawn Enemy
            enemyController.initEnemies();
            // Remove GameObject
            foreach (var gameObject1 in gameObjects) {
                Destroy(gameObject1);
                /*gameObject1.GetComponent<SpriteRenderer>().sprite = null;
                gameObject1.GetComponent<CircleCollider2D>().enabled = false;*/
            }
        }
    }

    public GameObject obj;
    public List<waypointController> waypointControllers;
    public Waypoint waypoint;

    private void Awake() {
        waypointControllers = new List<waypointController>();
        waypoint = new Waypoint(
            this.GetComponent<Transform>().position,
            this.name,
            layerMask
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
