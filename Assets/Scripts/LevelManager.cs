using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    ColliderGenerator colliderGenerator = default;
    [SerializeField]
    BackgroundGenerator backgroundGenerator = default;
    [SerializeField]
    FrameGenerator frameGenerator = default;
    [SerializeField]
    CollectibleGenerator collectibleGenerator = default;
    [SerializeField]
    PlayerNavigation player = default;

    [SerializeField]
    Tilemap heightReferenceTilemap = default;
    [SerializeField]
    Transform heightPos = default;
    [SerializeField, Range(0f, 1f)]
    float acceleration = 0.01f;

    IList<LevelMovement> levelsToMove = new List<LevelMovement>();

    int currentHeight {
        get {
            return heightReferenceTilemap.WorldToCell(heightPos.position).y;
        }
    }

    protected void Awake() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
        if (!colliderGenerator) {
            colliderGenerator = transform.GetComponentInChildren<ColliderGenerator>();
        }
        if (!backgroundGenerator) {
            backgroundGenerator = transform.GetComponentInChildren<BackgroundGenerator>();
        }
        if (!frameGenerator) {
            frameGenerator = transform.GetComponentInChildren<FrameGenerator>();
        }
        if (!collectibleGenerator) {
            collectibleGenerator = transform.GetComponentInChildren<CollectibleGenerator>();
        }
    }

    protected void OnValidate() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
        if (!colliderGenerator) {
            colliderGenerator = transform.GetComponentInChildren<ColliderGenerator>();
        }
        if (!backgroundGenerator) {
            backgroundGenerator = transform.GetComponentInChildren<BackgroundGenerator>();
        }
        if (!frameGenerator) {
            frameGenerator = transform.GetComponentInChildren<FrameGenerator>();
        }
        if (!collectibleGenerator) {
            collectibleGenerator = transform.GetComponentInChildren<CollectibleGenerator>();
        }
    }

    protected void Start() {
        foreach (var level in levelsToMove) {
            level.SetAcceleration(acceleration);
        }
        player.SetAcceleration(acceleration);
    }

    protected void FixedUpdate() {
        //startingAcceleration += accelerationFactor;
        foreach (var level in levelsToMove) {
            level.SetAcceleration(acceleration);
        }
        player.SetAcceleration(acceleration);
    }

    public void MoveLevel() {
        foreach (var level in levelsToMove) {
            level.StartMoving();
        }
        colliderGenerator.StartSpawning();
        backgroundGenerator.StartSpawning();
        frameGenerator.StartSpawning();
        collectibleGenerator.StartSpawning();
    }

    public void StopLevel() {
        foreach (var level in levelsToMove) {
            level.StopMoving();
        }
        colliderGenerator.StopSpawning();
        backgroundGenerator.StopSpawning();
        frameGenerator.StopSpawning();
        collectibleGenerator.StopSpawning();
    }

    internal void AdjustSpeedWithScore(int points) {
        foreach (var level in levelsToMove) {
            level.SubtractFromMovementSpeed(points);
        }
        player.SubtractFromMovementSpeed(points);
    }
}
