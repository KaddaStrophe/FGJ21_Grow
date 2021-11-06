using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class BackgroundGenerator : MonoBehaviour {
    [SerializeField]
    RuleTile tileToInstantiate = default;
    [SerializeField]
    Tilemap tilemap = default;
    [SerializeField]
    Transform spawnRoot = default;
    [SerializeField, Range(1, 100)]
    int levelWidth = 18;
    [SerializeField, Range(1, 10)]
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
            var targetGroundGridPosition = tilemap.WorldToCell(spawnRoot.position);
            for (; lastSpawnPos < targetGroundGridPosition.y + spawnHeight; lastSpawnPos++) {
                SpawnBackground(lastSpawnPos);
            }
        }
    }
    void SpawnBackground(int y) {
        for (int i = levelWidth; i > 0; i--) {
            if (!tilemap.HasTile(new Vector3Int(i - 1, y, 0))) {
                tilemap.SetTile(new Vector3Int(i - 1, y, 0), tileToInstantiate);
            }
        }
    }
    public void StartSpawning() {
        isSpawning = true;
    }
    public void StopSpawning() {
        isSpawning = false;
    }
}
