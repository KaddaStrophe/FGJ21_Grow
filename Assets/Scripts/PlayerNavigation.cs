using System;
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
    Tilemap frameTilemap = default;
    [SerializeField]
    Tilemap plantTilemap = default;
    [SerializeField]
    Tilemap flowerTilemap = default;
    [SerializeField]
    RuleTile plantTile = default;
    [SerializeField]
    RuleTile flowerTile = default;
    [SerializeField, Range(0f, 1f)]
    float snapThreshold = 0.5f;

    [SerializeField, Range(0f, 4f)]
    float stepTime = 1f;
    [SerializeField, Range(0f, 4f)]
    float maxStepTime = 1f;
    [SerializeField, Range(0f, 5f)]
    float accelerationFactor = 2.5f;
    [SerializeField]
    bool alwaysMove = false;
    [SerializeField]
    bool useFourDirections = true;

    [SerializeField]
    int score = 0;

    PlayerInput playerInput;
    Vector2 currentMovement;
    bool isActive = false;

    public static event Action onGameOver;

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
        Assert.IsTrue(flowerTilemap, nameof(flowerTilemap));
        playerInput.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        isActive = true;
        var currentGridPosition = plantTilemap.WorldToCell(player.position);
        plantTilemap.SetTile(currentGridPosition, plantTile);
        maxStepTime = stepTime;
    }

    float timer;

    void FixedUpdate() {
        if (!isActive) {
            return;
        }
        if (timer >= stepTime) {
            if (currentMovement != Vector2.zero && CanMove(currentMovement)) {
                timer = 0;
                levelManager.MoveLevel();
                MoveToNewPos(currentMovement);
            }
        } else {
            timer += Time.deltaTime;
        }
    }

    void Move(Vector2 direction) {
        currentMovement = direction;
        if (alwaysMove && currentMovement == Vector2.zero) {
            currentMovement = new Vector2(0, 1);
        }
        if (useFourDirections) {
            currentMovement = CalculateSnap4(currentMovement);
        } else {
            currentMovement = CalculateSnap8(currentMovement);
        }
    }

    public void SetAcceleration(float newAcceleration) {
        stepTime -= newAcceleration * Time.deltaTime * accelerationFactor;
    }

    public void SubtractFromMovementSpeed(float speed) {
        float tempStep = stepTime + (speed * Time.deltaTime * accelerationFactor);
        if(tempStep > maxStepTime) {
            tempStep = maxStepTime;
        }
        stepTime = tempStep;
    }

    void MoveToNewPos(Vector2 direction) {
        player.position += (Vector3)direction;
        var currentGridPosition = plantTilemap.WorldToCell(player.position);
        plantTilemap.SetTile(currentGridPosition, plantTile);

    }
    Vector2 CalculateSnap4(Vector2 analogInput) {
        if (analogInput.x > snapThreshold && analogInput.x > Mathf.Abs(analogInput.y)) {
            analogInput.x = 1f;
        } else if (analogInput.x < -snapThreshold && Mathf.Abs(analogInput.x) > Mathf.Abs(analogInput.y)) {
            analogInput.x = -1f;
        } else {
            analogInput.x = 0f;
        }

        if (analogInput.y > snapThreshold && analogInput.y > Mathf.Abs(analogInput.x)) {
            analogInput.y = 1f;
        } else if (analogInput.y < -snapThreshold && Mathf.Abs(analogInput.y) > Mathf.Abs(analogInput.x)) {
            analogInput.y = -1f;
        } else {
            analogInput.y = 0f;
        }
        return analogInput;
    }

    Vector2 CalculateSnap8(Vector2 analogInput) {
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
        var targetGroundGridPosition = groundTilemap.WorldToCell(player.position + (Vector3)direction);
        var targetColliderGridPosition = collisionTilemap.WorldToCell(player.position + (Vector3)direction);
        var targetPlantGridPosition = plantTilemap.WorldToCell(player.position + (Vector3)direction);
        var targetFrameGridPosition = frameTilemap.WorldToCell(player.position + (Vector3)direction);
        if (!groundTilemap.HasTile(targetGroundGridPosition)
            || collisionTilemap.HasTile(targetColliderGridPosition)
            || plantTilemap.HasTile(targetPlantGridPosition)
            || frameTilemap.HasTile(targetFrameGridPosition)) {
            if (collisionTilemap.HasTile(targetColliderGridPosition)) {
                // Game Over State
                MoveToNewPos(direction);
                DrawFlower(targetPlantGridPosition);
                GameOver();
            }
            return false;
        } else {
            return true;
        }
    }

    public void CommenceDeath() {
        GameOver();
    }

    void GameOver() {
        isActive = false;
        levelManager.StopLevel();
        onGameOver?.Invoke();
    }

    void DrawFlower(Vector3Int targetPlantGridPosition) {
        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                flowerTilemap.SetTile(targetPlantGridPosition + new Vector3Int(i, j, 0), flowerTile);
            }
        }

    }

    public void AddToScore(int points) {
        score += points;
        AdjustSpeed(points);
    }

    void AdjustSpeed(int points) {
        levelManager.AdjustSpeedWithScore(points);
    }
}