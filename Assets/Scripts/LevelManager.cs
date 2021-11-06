using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {
    [SerializeField]
    ColliderGenerator colliderGenerator = default;
    [SerializeField]
    BackgroundGenerator backgroundGenerator = default;

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
    }

    protected void OnValidate() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
        if (!colliderGenerator) {
            colliderGenerator = transform.GetComponentInChildren<ColliderGenerator>();
        }
        if (!backgroundGenerator) {
            backgroundGenerator = transform.GetComponentInChildren<BackgroundGenerator>();
        }
    }

    public void MoveLevel() {
        foreach (var level in levelsToMove) {
            level.StartMoving();
        }
        colliderGenerator.StartSpawning();
        backgroundGenerator.StartSpawning();
    }

    public void StopLevel() {
        foreach (var level in levelsToMove) {
            level.StopMoving();
        }
        colliderGenerator.StopSpawning();
        backgroundGenerator.StopSpawning();
    }

    void FixedUpdate() {
        //Debug.Log(currentHeight);
    }
}
