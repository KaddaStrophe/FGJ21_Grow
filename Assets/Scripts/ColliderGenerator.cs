using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class ColliderGenerator : MonoBehaviour {
    [SerializeField]
    Tile TileToInstantiate = default;
    [SerializeField]
    Tilemap tilemap = default;
    [SerializeField]
    Transform spawnRoot = default;
    [SerializeField, Range(1, 10)]
    int spawnHeight = 10;
    [SerializeField, Range(1, 20)]
    int minHeight = 10;
    [SerializeField]
    Transform[] spawnList = default;
    [SerializeField, Range(1, 10)]
    int minWidth = 1;
    [SerializeField, Range(1, 10)]
    int maxWidth = 3;

    bool isSpawning = false;
    int lastSpawnPos;


    protected void Start() {
        Assert.IsNotNull(spawnList, nameof(spawnList));
        Assert.IsTrue(tilemap, nameof(tilemap));
        lastSpawnPos = (int)spawnRoot.position.y + spawnHeight;
    }

    protected void FixedUpdate() {
        if (isSpawning) {
            for (; lastSpawnPos < spawnRoot.position.y + spawnHeight; lastSpawnPos++) {
                SpawnObstacle(lastSpawnPos);
            }
        }
    }

    void SpawnObstacle(int y) {
        var spawnHorPos = spawnList[UnityEngine.Random.Range(0, spawnList.Length)];
        int obstacleWidth = UnityEngine.Random.Range(minWidth, maxWidth);

        var spawnGridPosition = tilemap.WorldToCell(spawnHorPos.position);
        spawnGridPosition.y = y;
        tilemap.SetTile(spawnGridPosition, TileToInstantiate);
        if (obstacleWidth > minWidth) {
            bool toLeft = true;
            int leftStep = 1;
            int rightStep = 1;
            for (int i = obstacleWidth; i > minWidth; i--) {
                if (toLeft) {
                    spawnGridPosition = tilemap.WorldToCell(spawnHorPos.position) - new Vector3Int(leftStep, 0, 0);
                    spawnGridPosition.y = y;
                    if (CanSpawn(spawnGridPosition)) {
                        tilemap.SetTile(spawnGridPosition, TileToInstantiate);
                        leftStep++;
                    } else {
                        toLeft = !toLeft;
                    }
                } else {
                    spawnGridPosition = tilemap.WorldToCell(spawnHorPos.position) + new Vector3Int(rightStep, 0, 0);
                    spawnGridPosition.y = y;
                    if (CanSpawn(spawnGridPosition)) {
                        tilemap.SetTile(spawnGridPosition, TileToInstantiate);
                        leftStep++;
                    } else {
                        toLeft = !toLeft;
                    }
                }
                toLeft = !toLeft;
            }
        }
    }

    bool CanSpawn(Vector3Int spawnGridPosition) {
        return !tilemap.HasTile(spawnGridPosition);
    }

    public void StartSpawning() {
        isSpawning = true;
    }
    public void StopSpawning() {
        isSpawning = false;
    }
}
