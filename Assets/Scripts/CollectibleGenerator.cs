using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class CollectibleGenerator : MonoBehaviour {
    [SerializeField]
    RuleTile tileToInstantiate = default;
    [SerializeField, Range(0f, 1f)]
    float spawnRate = 0.2f;
    [SerializeField]
    Tilemap attachedTilemap = default;
    [SerializeField]
    Tilemap colliderTilemap = default;
    [SerializeField]
    Transform spawnRoot = default;
    [SerializeField, Range(10, 15)]
    int spawnHeight = 10;
    [SerializeField]
    Transform[] spawnList = default;

    bool isSpawning = false;
    int lastSpawnPos;


    protected void Start() {
        Assert.IsNotNull(spawnList, nameof(spawnList));
        Assert.IsTrue(attachedTilemap, nameof(attachedTilemap));
        Assert.IsTrue(colliderTilemap, nameof(colliderTilemap));
        lastSpawnPos = (int)spawnRoot.position.y + spawnHeight;
    }

    protected void FixedUpdate() {
        if (isSpawning) {
            var targetGroundGridPosition = colliderTilemap.WorldToCell(spawnRoot.position);
            for (; lastSpawnPos < targetGroundGridPosition.y + spawnHeight; lastSpawnPos++) {
                if (UnityEngine.Random.Range(0f, 1f) <= spawnRate) {
                    SpawnCollectible(lastSpawnPos);
                }
            }
        }
    }

    void SpawnCollectible(int y) {
        var spawnHorPos = spawnList[UnityEngine.Random.Range(0, spawnList.Length)];

        var spawncolliderGridPosition = colliderTilemap.WorldToCell(spawnHorPos.position);
        spawncolliderGridPosition.y = y;
        var spawnGridPosition = attachedTilemap.WorldToCell(spawnHorPos.position);
        spawnGridPosition.y = y;

        if (CanSpawn(colliderTilemap, spawncolliderGridPosition)) {
            attachedTilemap.SetTile(spawnGridPosition, tileToInstantiate);
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
