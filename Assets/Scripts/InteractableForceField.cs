using UnityEngine;

public class InteractableForceField : MonoBehaviour {
    [SerializeField, Range(0, 100)]
    float force = 1;

    void OnTriggerStay(Collider other) {
        if (other.TryGetComponent<Interactable>(out var interactable)) {
            var direction = transform.position - interactable.transform.position;
            direction.Normalize();
            direction *= force;
            direction *= Time.deltaTime;
            interactable.MoveBy(direction);
        }
    }
}
