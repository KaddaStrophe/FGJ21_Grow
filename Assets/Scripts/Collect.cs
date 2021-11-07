using UnityEngine;

public class Collect : MonoBehaviour {
    [SerializeField]
    Collider2D attachedCollider = default;
    [SerializeField, Range(0,10)]
    int point = 1;

    protected void OnValidate() {
        if (!attachedCollider) {
            attachedCollider = transform.GetComponentInChildren<Collider2D>();
        }
    }

    protected void Awake() {
        if (!attachedCollider) {
            attachedCollider = transform.GetComponentInChildren<Collider2D>();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.TryGetComponent<PlayerNavigation>(out var player);
            player.Collect(point);
        }
    }
}
