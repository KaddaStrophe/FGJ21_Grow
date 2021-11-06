using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class ColliderGenerator : MonoBehaviour {
    [SerializeField]
    Tile TileToInstantiate = default;
    [SerializeField]
    Tilemap tilemap = default;
    [SerializeField]
    Transform[] spawnList = default;
    [SerializeField, Range(1f, 10f)]
    float beginningSpawnRate = 4;
    [SerializeField, Range(1, 10)]
    int minWidth = 1;
    [SerializeField, Range(1, 10)]
    int maxWidth = 3;

    float currentSpawRate;
    bool isSpawning = false;
    bool startedSpawning = false;


    protected void Start() {
        Assert.IsNotNull(spawnList, nameof(spawnList));
        Assert.IsTrue(tilemap, nameof(tilemap));
        currentSpawRate = beginningSpawnRate;
    }

    protected void FixedUpdate() {
        if (isSpawning && !startedSpawning) {
            startedSpawning = true;
            StartCoroutine(SpawnObstaclesRandom());
        }
    }
    IEnumerator SpawnObstaclesRandom() {
        while (isSpawning) {
            yield return new WaitForSeconds(currentSpawRate);
            var spawnPos = spawnList[UnityEngine.Random.Range(0, spawnList.Length)];
            int obstacleWidth = UnityEngine.Random.Range(minWidth, maxWidth);
            SpawnObstacle(spawnPos, obstacleWidth);
        }
        startedSpawning = false;
        StopCoroutine(SpawnObstaclesRandom());
    }

    void SpawnObstacle(Transform spawnPos, int obstacleWidth) {
        var spawnGridPosition = tilemap.WorldToCell(spawnPos.position);
        tilemap.SetTile(spawnGridPosition, TileToInstantiate);
        if (obstacleWidth > minWidth) {
            bool toLeft = true;
            int leftStep = 1;
            int rightStep = 1;
            for (int i = obstacleWidth; i > minWidth; i--) {
                if (toLeft) {
                    spawnGridPosition = tilemap.WorldToCell(spawnPos.position) - new Vector3Int(leftStep, 0, 0);
                    if (CanSpawn(spawnGridPosition)) {
                        tilemap.SetTile(spawnGridPosition, TileToInstantiate);
                        leftStep++;
                    } else {
                        toLeft = !toLeft;
                    }
                } else {
                    spawnGridPosition = tilemap.WorldToCell(spawnPos.position) + new Vector3Int(rightStep, 0, 0);
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
        currentSpawRate = beginningSpawnRate;
        isSpawning = true;
    }
    public void StopSpawning() {
        isSpawning = false;
    }

    public void SetSpawnRate(float spawnRate) {
        currentSpawRate = spawnRate;
    }
}
