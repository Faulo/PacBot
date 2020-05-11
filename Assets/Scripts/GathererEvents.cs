using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

public class GathererEvents : MonoBehaviour {
    [Serializable]
    class InteractableEvent : UnityEvent<Interactable> { }

    [SerializeField, Expandable]
    Gatherer observedGatherer = default;

    [SerializeField]
    InteractableEvent onCollect = new InteractableEvent();

    void OnEnable() {
        observedGatherer.onCollect += onCollect.Invoke;
    }
    void OnDisable() {
        observedGatherer.onCollect -= onCollect.Invoke;
    }
}
