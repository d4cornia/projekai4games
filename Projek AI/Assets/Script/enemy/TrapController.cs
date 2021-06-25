using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            var controller = collision.gameObject.GetComponent<playerController>();
            controller.slowPlayer();
            Destroy(this.gameObject);
        }
    }
}
