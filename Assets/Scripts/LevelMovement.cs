using System;
using UnityEngine;

public class LevelMovement : MonoBehaviour {
    [SerializeField, Range(0f, 10f)]
    float speed = 1f;
    [SerializeField]
    Transform objectToMove = default;

    bool shouldMove = false;

    public float currentSpeed { get { return speed; } }

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

    protected void FixedUpdate() {
        if (shouldMove) {
            objectToMove.Translate(0, -Time.deltaTime * speed, 0);
        }
    }

    public void StartMoving() {
        shouldMove = true;
    }

    public void StopMoving() {
        shouldMove = false;
    }

    public void AddToMovementSpeed(float speed) {
        this.speed += speed;
    }
}
