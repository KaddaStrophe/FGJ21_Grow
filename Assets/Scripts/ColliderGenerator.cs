using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class ColliderGenerator : MonoBehaviour {
    [SerializeField]
    RuleTile tileToInstantiate = default;
    [SerializeField]
    Tilemap colliderTilemap = default;
    [SerializeField]
    Tilemap frameTilemap = default;
    [SerializeField]
    Transform spawnRoot = default;
    [SerializeField, Range(10, 15)]
    int spawnHeight = 10;
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
        Assert.IsTrue(colliderTilemap, nameof(colliderTilemap));
        Assert.IsTrue(frameTilemap, nameof(frameTilemap));
        lastSpawnPos = (int)spawnRoot.position.y + spawnHeight;
    }

    protected void FixedUpdate() {
        if (isSpawning) {
            var targetGroundGridPosition = colliderTilemap.WorldToCell(spawnRoot.position);
            for (; lastSpawnPos < targetGroundGridPosition.y + spawnHeight; lastSpawnPos++) {
                SpawnObstacle(lastSpawnPos);
            }
        }
    }

    void SpawnObstacle(int y) {
        var spawnHorPos = spawnList[UnityEngine.Random.Range(0, spawnList.Length)];
        int obstacleWidth = UnityEngine.Random.Range(minWidth, maxWidth + 1);

        bool toLeft = true;
        int leftStep = 1;
        int rightStep = 1;

        var spawncolliderGridPosition = colliderTilemap.WorldToCell(spawnHorPos.position);
        spawncolliderGridPosition.y = y;
        colliderTilemap.SetTile(spawncolliderGridPosition, tileToInstantiate);

        for (int i = obstacleWidth - 1; i > 0; i--) {
            if (toLeft) {
                spawncolliderGridPosition = colliderTilemap.WorldToCell(spawnHorPos.position) - new Vector3Int(leftStep, 0, 0);
                spawncolliderGridPosition.y = y;
                var spawnframeGridPosition = frameTilemap.WorldToCell(spawnHorPos.position) - new Vector3Int(leftStep, 0, 0);
                spawnframeGridPosition.y = y;
                if (CanSpawn(colliderTilemap, spawncolliderGridPosition) && CanSpawn(frameTilemap, spawnframeGridPosition)) {
                    colliderTilemap.SetTile(spawncolliderGridPosition, tileToInstantiate);
                    leftStep++;
                }
            } else {
                spawncolliderGridPosition = colliderTilemap.WorldToCell(spawnHorPos.position) + new Vector3Int(rightStep, 0, 0);
                spawncolliderGridPosition.y = y;
                var spawnframeGridPosition = frameTilemap.WorldToCell(spawnHorPos.position) + new Vector3Int(rightStep, 0, 0);
                spawnframeGridPosition.y = y;
                if (CanSpawn(colliderTilemap, spawncolliderGridPosition) && CanSpawn(frameTilemap, spawnframeGridPosition)) {
                    colliderTilemap.SetTile(spawncolliderGridPosition, tileToInstantiate);
                    rightStep++;
                }
            }
            toLeft = !toLeft;
        }
    }

    bool CanSpawn(Tilemap tilemap, Vector3Int spawnGridPosition) {
        return !tilemap.HasTile(spawnGridPosition);
    }

    public void StartSpawning() {
        isSpawning = true;
    }
    public void StopSpawning() {
        isSpawning = false;
    }
}
