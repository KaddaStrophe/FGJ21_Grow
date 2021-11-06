using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    ColliderGenerator colliderGenerator = default;

    IList<LevelMovement> levelsToMove = new List<LevelMovement>();


    protected void Awake() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
        if(!colliderGenerator) {
            colliderGenerator = transform.GetComponentInChildren<ColliderGenerator>();
        }
    }

    protected void OnValidate() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
        if (!colliderGenerator) {
            colliderGenerator = transform.GetComponentInChildren<ColliderGenerator>();
        }
    }

    public void MoveLevel() { 
        foreach(var level in levelsToMove) {
            level.StartMoving();
        }
        colliderGenerator.StartSpawning();
    }

    public void StopLevel() {
        foreach (var level in levelsToMove) {
            level.StopMoving();
        }
        colliderGenerator.StopSpawning();
    }
}
