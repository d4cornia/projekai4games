using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphWaypointController : MonoBehaviour
{
    List<WaypointController> waypoints = new List<WaypointController>();

    public void addWaypoint(WaypointController waypointController) {
        waypoints.Add(waypointController);
    }

    WaypointController getNearestWaypoint(Vector2 origin, bool isWallIgnore = false) {
        WaypointController closestWaypoint = null;
        float closestDistance = 0;
        foreach (var waypoint in this.waypoints) {
            Vector2 wpPos = waypoint.transform.position;
            var dist = Vector2.Distance(origin, wpPos);
            if (closestWaypoint != null) {
                int layerMask = 1 << LayerMask.NameToLayer("wall"); // TODO implementasi layermask
                var raycast = Physics2D.Raycast(origin, (wpPos - origin).normalized, 100, layerMask);
                bool isPossible = isWallIgnore || raycast.collider == null; // Check Raycast
                bool isCloser = dist < closestDistance;
                if (!isPossible || !isCloser) {
                    continue;
                }
            }
            // Jika valid maka update closest
            closestWaypoint = waypoint;
            closestDistance = dist;
        }
        return closestWaypoint;
    }

    public Vector2 getNearestWaypointPos(Vector2 origin, bool isWallIgnore = false) {
        var waypoint = getNearestWaypoint(origin, isWallIgnore);
        return waypoint.transform.position;
    }
    public Vector2 getRandomNeighbourPos(Vector2 origin, bool isWallIgnore = false) {
        var waypoint = getNearestWaypoint(origin, isWallIgnore);
        int randomIdx = Random.Range(0, waypoint.Neighbours.Count);
        var neighbour = waypoint.Neighbours[randomIdx];
        return neighbour.transform.position;
    }
    public Vector2 getRandomWaypointPos(bool isWallIgnore = false) { // Get Random Position
        int randomIdx = Random.Range(0, waypoints.Count - 1);
        var waypoint = waypoints[randomIdx];
        return waypoint.transform.position;
    }
}
