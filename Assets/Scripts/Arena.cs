using System;
using UnityEngine;

public class Arena : MonoBehaviour {
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

    Transform[,] tiles;

    [Header("Level layout")]
    [SerializeField]
    LayoutMode mode = default;
    [SerializeField, Range(0, 100)]
    int height = 5;
    [SerializeField, Range(0, 1)]
    float pathY = 0.5f;
    [SerializeField, Range(0, 1)]
    float pathMargin = 0.1f;

    void Awake() {
        transform.localPosition = new Vector3(width / -2, 0, depth / -2);

        tiles = new Transform[width, depth];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                Debug.Log($"({x}, {z}): {Mathf.PerlinNoise(x, z)}");
                tiles[x, z] = Instantiate(wallPrefab, transform);
            }
        }
    }

    void Update() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                float y = Mathf.PerlinNoise((float)x, (float)z);
                tiles[x, z].localPosition = new Vector3(x, CalculateY(y), z);
            }
        }
    }
    float CalculateY(float noise) {
        switch (mode) {
            case LayoutMode.HeightMap:
                return Mathf.RoundToInt(height * noise);
            case LayoutMode.HeightPath:
                if (noise < pathY - pathMargin || noise > pathY + pathMargin) {
                    return 1;
                } else {
                    return 0;
                }
            default:
                throw new NotImplementedException(mode.ToString());
        }
    }
}
