using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

public class BrainEvents : MonoBehaviour {
    [Serializable]
    class BrainEvent : UnityEvent<Brain> { }

    [SerializeField, Expandable]
    Brain observedBrain = default;

    [SerializeField]
    BrainEvent onEpisodeBegin = new BrainEvent();
    [SerializeField]
    BrainEvent onAction = new BrainEvent();

    void OnEnable() {
        if (!observedBrain) {
            observedBrain = GetComponentInParent<Brain>();
        }
        observedBrain.onEpisodeBegin += onEpisodeBegin.Invoke;
        observedBrain.onAction += onAction.Invoke;
    }
    void OnDisable() {
        observedBrain.onEpisodeBegin -= onEpisodeBegin.Invoke;
        observedBrain.onAction -= onAction.Invoke;
    }
}
