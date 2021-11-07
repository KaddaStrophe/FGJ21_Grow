using System.Collections.Generic;
using TMPro;
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
    TextMeshProUGUI heightDisplay = default;

    [SerializeField]
    Tilemap heightReferenceTilemap = default;
    [SerializeField, Range(0f, 1f)]
    float acceleration = 0.01f;

    IList<LevelMovement> levelsToMove = new List<LevelMovement>();
    

    public int currentHeight {
        get {
            return heightReferenceTilemap.WorldToCell(player.transform.position).y;
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

    public void AdjustSpeedWithScore(int points) {
        foreach (var level in levelsToMove) {
            level.SubtractFromMovementSpeed(points);
        }
        player.SubtractFromMovementSpeed(points);
    }

    protected void OnEnable() {
        PlayerNavigation.onGameOver += SetHeightDisplay;
    }
    protected void OnDisable() {
        PlayerNavigation.onGameOver -= SetHeightDisplay;
    }

    void SetHeightDisplay() {
        heightDisplay.text = "Your score: " + currentHeight.ToString() + "m";
    }
}
