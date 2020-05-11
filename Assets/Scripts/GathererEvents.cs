using System;
using UnityEngine;
using UnityEngine.Events;

public class GathererEvents : MonoBehaviour {
    [Serializable]
    class InteractableEvent : UnityEvent<Interactable> { }

    [SerializeField]
    Gatherer observedGatherer = default;

    [SerializeField]
    InteractableEvent onCollect = default;

    void OnEnable() {
        observedGatherer.onCollect += onCollect.Invoke;
    }
    void OnDisable() {
        observedGatherer.onCollect -= onCollect.Invoke;
    }
}
