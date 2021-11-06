using UnityEngine;

public class LevelMovement : MonoBehaviour {
    [SerializeField, Range(0f, 10f)]
    float speed = 1f;
    [SerializeField]
    Transform objectToMove = default;

    float currentSpeed = 0f;

    protected void Awake() {
        if (!objectToMove) {
            objectToMove = transform;
        }
    }

    protected void OnValidate() {
        if (!objectToMove) {
            objectToMove = transform;
        }
    }

    protected void Update() {
        objectToMove.Translate(0, -Time.deltaTime * currentSpeed, 0);
    }

    public void StartMoving() {
        currentSpeed = speed;
    }

    public void StopMoving() {
        currentSpeed = 0f;
    }
}
