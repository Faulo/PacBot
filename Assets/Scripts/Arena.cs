using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

[SelectionBase]
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
    [Serializable]
    class AcademyMapGenerator : ILevelGenerator {
        [SerializeField]
        string heightParameter = "map_height";
        public float GetY(float noise) {
            return Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault(heightParameter, 0) * noise);
        }
    }
    

    enum LayoutMode {
        HeightMap,
        HeightPath,
        AcademyMap,
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

    [SerializeField]
    Transform botPrefab = default;

    Transform[,] tiles;
    ISet<Transform> interactables;

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
                case LayoutMode.AcademyMap:
                    return academyMap;
                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }
    }
    [SerializeField]
    HeightMapGenerator heightMap = default;
    [SerializeField]
    HeightPathGenerator heightPath = default;
    [SerializeField]
    AcademyMapGenerator academyMap = default;
    [SerializeField]
    bool updateTilesWhileRunning = false;
    [SerializeField, Range(0, 1)]
    float updateInterval = 1;

    void Awake() {
        transform.localPosition += new Vector3(width / -2, 0, depth / -2);

        tiles = new Transform[width, depth];
        interactables = new HashSet<Transform>();
        int t = 0;
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                tiles[x, z] = Instantiate(wallPrefab, transform);
                if (t % interactablesPerTile == 0) {
                    interactables.Add(Instantiate(interactablePrefab, tiles[x, z]));
                }
                UpdateTileAt(x, z);
                t++;
            }
        }
        int botX = width / 2;
        int botZ = depth / 2;
        var bot = Instantiate(botPrefab, tiles[botX, botZ].position + Vector3.up, Quaternion.identity, transform);
        bot.GetComponentInChildren<Brain>().onEpisodeBegin += ResetArena;
    }

    void ResetArena(Brain bot) {
        foreach (var interactable in interactables) {
            interactable.localPosition = interactablePrefab.localPosition;
            interactable.gameObject.SetActive(true);
        }
        UpdateTiles();
    }

    void UpdateTiles() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                UpdateTileAt(x, z);
            }
        }
    }
    void UpdateTileAt(int x, int z) {
        float y = Mathf.PerlinNoise((x / noiseScale) + noiseOffsetX + transform.position.x, (z / noiseScale) + noiseOffsetZ + transform.position.z);
        tiles[x, z].localPosition = new Vector3(x, generator.GetY(y), z);
    }

    IEnumerator Start() {
        while (updateTilesWhileRunning) {
            UpdateTiles();
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
