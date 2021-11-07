using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.TryGetComponent<PlayerNavigation>(out var player);
            player.CommenceDeath();
        }
    }
}
