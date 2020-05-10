using System;
using UnityEngine;

public class Gatherer : MonoBehaviour {

    public event Action<Interactable> onCollect;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Interactable>(out var interactable)) {
            onCollect?.Invoke(interactable);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Interactable>(out var interactable)) {
            onCollect?.Invoke(interactable);
            Destroy(interactable.gameObject);
        }
    }
}
