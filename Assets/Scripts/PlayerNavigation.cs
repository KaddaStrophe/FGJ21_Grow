using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class PlayerNavigation : MonoBehaviour {
    [SerializeField]
    Transform player = default;
    [SerializeField]
    Tilemap groundTilemap = default;
    [SerializeField]
    Tilemap collisionTilemap = default;
    [SerializeField]
    Tilemap plantTilemap = default;
    [SerializeField]
    Tile plantTile = default;
    [SerializeField, Range(0f, 1f)]
    float snapThreshold = 0.5f;
    [SerializeField, Range(0f, 4f)]
    float stepTime = 1f;
    [SerializeField]
    bool alwaysMove = false;

    PlayerInput playerInput;
    bool isMoving = false;
    bool startedMovement = false;
    Vector2 currentMovement;

    protected void OnEnable() {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    protected void OnDisable() {
        playerInput.Disable();
    }

    void Start() {
        Assert.IsTrue(player, nameof(player));
        Assert.IsTrue(groundTilemap, nameof(groundTilemap));
        Assert.IsTrue(collisionTilemap, nameof(collisionTilemap));
        Assert.IsTrue(plantTilemap, nameof(plantTilemap));
        playerInput.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    void Move(Vector2 direction) {
        isMoving = true;
        currentMovement = direction;
        if (!startedMovement) {
            startedMovement = true;
            StartCoroutine(MoveHead());
        }

    }

    IEnumerator MoveHead() {
        while (isMoving) {
            yield return new WaitForSeconds(stepTime);
            currentMovement = CalculateSnap(currentMovement);
            if (alwaysMove && currentMovement == Vector2.zero) {
                currentMovement = new Vector2(0, 1);
            }
            if (!(currentMovement == Vector2.zero) && CanMove(currentMovement)) {
                var currentGridPosition = plantTilemap.WorldToCell(transform.position);
                plantTilemap.SetTile(currentGridPosition, plantTile);
                transform.position += (Vector3)currentMovement;
            }
        }
        startedMovement = false;
        StopCoroutine(MoveHead());
    }

    Vector2 CalculateSnap(Vector2 analogInput) {
        if (analogInput.x > snapThreshold) {
            analogInput.x = 1f;
        } else if (analogInput.x < -snapThreshold) {
            analogInput.x = -1f;
        } else {
            analogInput.x = 0f;
        }

        if (analogInput.y > snapThreshold) {
            analogInput.y = 1f;
        } else if (analogInput.y < -snapThreshold) {
            analogInput.y = -1f;
        } else {
            analogInput.y = 0f;
        }

        return analogInput;
    }

    bool CanMove(Vector2 direction) {
        var targetGridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(targetGridPosition) || collisionTilemap.HasTile(targetGridPosition)) {
            if (collisionTilemap.HasTile(targetGridPosition)) {
                Explode();
            }
            return false;
        } else {
            if (plantTilemap.HasTile(targetGridPosition)) {
                Bloom();
            }
            return true;
        }
    }

    void Bloom() {
        // TODO: Bloom other tile at interconnection
        //Debug.Log("Bloom");
    }

    void Explode() {
        // TODO: Explode into Flower
        // TODO: End Run / Menu (Back to Menu, Retry)
        // TODO: Display Meters
        //Debug.Log("Explode");
    }
}