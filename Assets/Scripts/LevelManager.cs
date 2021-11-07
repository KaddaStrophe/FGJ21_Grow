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
    Tilemap heightReferenceTilemap = default;
    [SerializeField]
    Transform heightPos = default;

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
    }

    public void MoveLevel() {
        foreach (var level in levelsToMove) {
            level.StartMoving();
        }
        colliderGenerator.StartSpawning();
        backgroundGenerator.StartSpawning();
        frameGenerator.StartSpawning();
    }

    public void StopLevel() {
        foreach (var level in levelsToMove) {
            level.StopMoving();
        }
        colliderGenerator.StopSpawning();
        backgroundGenerator.StopSpawning();
        frameGenerator.StopSpawning();
    }

    void FixedUpdate() {
        //Debug.Log(currentHeight);
    }
}
