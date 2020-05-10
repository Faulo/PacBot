using UnityEngine;

public class Arena : MonoBehaviour {
    [Header("Level")]
    [SerializeField, Range(0, 100)]
    int width = 10;
    [SerializeField, Range(0, 100)]
    int height = 5;
    [SerializeField, Range(0, 100)]
    int depth = 10;
    public Vector2Int size => new Vector2Int(width, depth);

    [SerializeField]
    Transform wallPrefab = default;

    void Awake() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                float y = Mathf.RoundToInt(height * Mathf.PerlinNoise((float)x / width, (float)z / depth));

                var wall = Instantiate(wallPrefab, transform);
                wall.localPosition = new Vector3(x - width / 2, y - height / 2, z - depth / 2);
            }
        }
    }
}
