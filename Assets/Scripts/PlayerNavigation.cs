using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class PlayerNavigation : MonoBehaviour {
    [SerializeField]
    Transform player = default;
    [SerializeField]
    LevelManager levelManager = default;
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
    Vector2 currentMovement;
    bool characterShouldMove = false;
    bool startedMovement = false;
    bool isActive = false;

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
        isActive = true;
    }

    void Move(Vector2 direction) {
        if (isActive && (alwaysMove || direction != Vector2.zero)) {
            characterShouldMove = true;
        } else if (!isActive || (direction == Vector2.zero && !alwaysMove)) {
            characterShouldMove = false;
        }
        if (characterShouldMove) {
            // Start moving level maps
            levelManager.MoveLevel();

            // Attempt to move Player
            currentMovement = direction;
            if (!startedMovement) {
                startedMovement = true;
                StartCoroutine(MoveHead());
            }
        }
    }

    IEnumerator MoveHead() {
        while (characterShouldMove) {
            yield return new WaitForSeconds(stepTime);
            currentMovement = CalculateSnap(currentMovement);
            if (alwaysMove && currentMovement == Vector2.zero) {
                currentMovement = new Vector2(0, 1);
            }
            if (CanMove(currentMovement)) {
                var currentGridPosition = plantTilemap.WorldToCell(transform.position);
                MoveToNewPos(currentGridPosition);
            }
        }
        startedMovement = false;
    }

    void MoveToNewPos(Vector3Int targetGridPosition) {
        plantTilemap.SetTile(targetGridPosition, plantTile);
        transform.position += (Vector3)currentMovement;
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
        var targetGroundGridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        var targetColliderGridPosition = collisionTilemap.WorldToCell(transform.position + (Vector3)direction);
        var targetPlantGridPosition = plantTilemap.WorldToCell(transform.position);
        if (!groundTilemap.HasTile(targetGroundGridPosition) || collisionTilemap.HasTile(targetColliderGridPosition)) {
            if (collisionTilemap.HasTile(targetColliderGridPosition)) {
                // Game Over State
                MoveToNewPos(targetPlantGridPosition);
                GameOver();
            }
            return false;
        } else {
            if (plantTilemap.HasTile(targetGroundGridPosition)) {
                Bloom();
            }
            return true;
        }
    }

    void Bloom() {
        // TODO: Bloom other tile at interconnection
        //Debug.Log("Bloom");
    }

    void GameOver() {
        isActive = false;
        characterShouldMove = false;
        levelManager.StopLevel();
        // TODO: Explode into Flower
        // TODO: End Run / Menu (Back to Menu, Retry)
        // TODO: Display Meters
    }
}