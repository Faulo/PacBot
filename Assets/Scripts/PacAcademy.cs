using System.Collections;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class PacAcademy : MonoBehaviour {
    [SerializeField, Expandable]
    Arena arenaPrefab = default;
    [SerializeField, Range(0, 100)]
    int arenaRows = 1;
    [SerializeField, Range(0, 100)]
    int arenaColumns = 1;

    IEnumerator Start() {
        var offset = 2 * arenaPrefab.size;
        for (int x = 0; x < arenaColumns; x++) {
            for (int y = 0; y < arenaRows; y++) {
                Instantiate(arenaPrefab, new Vector3(x * offset.x, 0, y * offset.y), Quaternion.identity, transform);
                yield return null;
            }
        }
    }
}
