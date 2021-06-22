using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [SerializeField] private GraphWaypointController graphParent;
    [SerializeField] private List<WaypointController> neighbours;

    public List<WaypointController> Neighbours { get => this.neighbours; } // Tetangga

    // Get All Waypoint
    void Start(){
        // Tambah Waypoint ke Graph (Parent)
        graphParent.addWaypoint(this);
        // Disable Sprite Render
        var renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }
}
