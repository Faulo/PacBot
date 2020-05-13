using System;
using UnityEngine;
using UnityEngine.Events;

public class ArenaEvents : MonoBehaviour {
    [Serializable]
    class ArenaEvent : UnityEvent<Arena> { }

    [SerializeField]
    Arena observedArena = default;

    [SerializeField]
    ArenaEvent onReset = new ArenaEvent();
    [SerializeField]
    ArenaEvent onClear = new ArenaEvent();

    void OnEnable() {
        if (!observedArena) {
            observedArena = GetComponentInParent<Arena>();
        }
        observedArena.onClear += onClear.Invoke;
        observedArena.onReset += onReset.Invoke;
    }
    void OnDisable() {
        observedArena.onClear -= onClear.Invoke;
        observedArena.onReset -= onReset.Invoke;
    }
}
