using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class FrameGenerator : MonoBehaviour {
    [SerializeField]
    RuleTile tileToInstantiate = default;
    [SerializeField]
    Tilemap tilemap = default;
    [SerializeField]
    Transform spawnRoot = default;
    [SerializeField, Range(1, 30)]
    int levelWidth = 18;
    [SerializeField, Range(10, 15)]
    int spawnHeight = 10;

    int lastSpawnPos;

    bool isSpawning = false;


    protected void Start() {
        Assert.IsNotNull(spawnRoot, nameof(spawnRoot));
        Assert.IsTrue(tileToInstantiate, nameof(tileToInstantiate));
        Assert.IsTrue(tilemap, nameof(tilemap));
    }

    protected void FixedUpdate() {
        if (isSpawning) {
            var targetFrameGridPosition = tilemap.WorldToCell(spawnRoot.position);
            for (; lastSpawnPos < targetFrameGridPosition.y + spawnHeight; lastSpawnPos++) {
                SpawnFrame(lastSpawnPos);
            }
        }
    }
    void SpawnFrame(int y) {
            tilemap.SetTile(new Vector3Int(levelWidth - 1, y, 0), tileToInstantiate);
            tilemap.SetTile(new Vector3Int(0, y, 0), tileToInstantiate);
    }
    public void StartSpawning() {
        isSpawning = true;
    }
    public void StopSpawning() {
        isSpawning = false;
    }
}
