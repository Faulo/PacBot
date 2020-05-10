using UnityEngine;

public class Interactable : MonoBehaviour {
    [SerializeField]
    public Rigidbody attachedRigidbody = default;

    void Update() {
        if (transform.position.y < -1) {
            Destroy(gameObject);
        }
    }
}
