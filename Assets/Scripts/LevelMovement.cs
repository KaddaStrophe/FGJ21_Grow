using System;
using UnityEngine;

public class LevelMovement : MonoBehaviour {
    [SerializeField, Range(0f, 10f)]
    float speed = 1f;
    [SerializeField]
    Transform objectToMove = default;
    [SerializeField, Range(0.1f, 20f)]
    public float minSpeed = 0.1f;
    [SerializeField, Range(0f, 1f)]
    public float acceleration = 0.1f;

    bool shouldMove = false;


    public float currentSpeed { get { return speed; } set { } }

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
            objectToMove.Translate(0, Vector2.down.y * Time.deltaTime * speed, 0);
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
    public void SubtractFromMovementSpeed(float speed) {
        if (this.speed - speed < minSpeed) {
            this.speed = minSpeed;
        } else {
            this.speed -= speed;
        }
    }

    public void SetAcceleration(float newAcceleration) {
        speed += newAcceleration;
    }
}
