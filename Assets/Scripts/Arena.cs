using System;
using System.Collections;
using UnityEngine;

public class Arena : MonoBehaviour {
    interface ILevelGenerator {
        float GetY(float noise);
    }
    [Serializable]
    class HeightMapGenerator : ILevelGenerator {
        [SerializeField, Range(0, 100)]
        int height = 5;
        public float GetY(float noise) {
            return Mathf.RoundToInt(height * noise);
        }
    }
    [Serializable]
    class HeightPathGenerator : ILevelGenerator {
        [SerializeField, Range(0, 1)]
        float pathY = 0.5f;
        [SerializeField, Range(0, 1)]
        float pathMargin = 0.1f;
        public float GetY(float noise) {
            if (noise < pathY - pathMargin || noise > pathY + pathMargin) {
                return 1;
            } else {
                return 0;
            }
        }
    }
    enum LayoutMode {
        HeightMap,
        HeightPath,
    }
    [Header("Level size")]
    [SerializeField, Range(0, 100)]
    int width = 10;
    [SerializeField, Range(0, 100)]
    int depth = 10;
    public Vector2Int size => new Vector2Int(width, depth);

    [SerializeField]
    Transform wallPrefab = default;
    [SerializeField]
    Transform interactablePrefab = default;
    [SerializeField, Range(1, 10)]
    float interactablesPerTile = 1;

    Transform[,] tiles;

    [Header("Level layout")]
    [SerializeField, Range(0.0001f, 1000)]
    float noiseScale = 1;
    [SerializeField, Range(0, 1000)]
    int noiseOffsetX = 0;
    [SerializeField, Range(0, 1000)]
    int noiseOffsetZ = 0;
    [SerializeField]
    LayoutMode mode = default;
    ILevelGenerator generator {
        get {
            switch (mode) {
                case LayoutMode.HeightMap:
                    return heightMap;
                case LayoutMode.HeightPath:
                    return heightPath;
                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }
    }
    [SerializeField]
    HeightMapGenerator heightMap = default;
    [SerializeField]
    HeightPathGenerator heightPath = default;
    [SerializeField, Range(0, 1)]
    float updateInterval = 1;

    void Awake() {
        transform.localPosition = new Vector3(width / -2, 0, depth / -2);

        tiles = new Transform[width, depth];
        int t = 0;
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                tiles[x, z] = Instantiate(wallPrefab, transform);
                if (t % interactablesPerTile == 0) {
                    Instantiate(interactablePrefab, tiles[x, z]);
                }
                t++;
            }
        }
    }

    IEnumerator Start() {
        while (true) {
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < depth; z++) {
                    float y = Mathf.PerlinNoise((x / noiseScale) + noiseOffsetX, (z / noiseScale) + noiseOffsetZ);
                    tiles[x, z].localPosition = new Vector3(x, generator.GetY(y), z);
                }
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
