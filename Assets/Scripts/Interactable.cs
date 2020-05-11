using UnityEngine;

public class Interactable : MonoBehaviour {
    [SerializeField]
    Rigidbody attachedRigidbody = default;

    public void MoveBy(Vector3 offset) {
        attachedRigidbody.MovePosition(attachedRigidbody.position + offset);
    }
}
