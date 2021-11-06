using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    IList<LevelMovement> levelsToMove = new List<LevelMovement>();

    protected void Awake() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
    }

    protected void OnValidate() {
        levelsToMove = transform.GetComponentsInChildren<LevelMovement>();
    }

    public void MoveLevel() { 
        foreach(var level in levelsToMove) {
            level.StartMoving();
        }
    }

    public void StopLevel() {
        foreach (var level in levelsToMove) {
            level.StopMoving();
        }
    }
}
