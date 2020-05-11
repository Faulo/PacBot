using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

public class MovementEvents : MonoBehaviour {
    [Serializable]
    class MovementEvent : UnityEvent<Movement> { }

    [SerializeField, Expandable]
    Movement observedMovement = default;

    [SerializeField]
    MovementEvent onFall = new MovementEvent();

    void OnEnable() {
        observedMovement.onFall += onFall.Invoke;
    }
    void OnDisable() {
        observedMovement.onFall -= onFall.Invoke;
    }
}
