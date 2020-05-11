using System;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

[SelectionBase]
public class Brain : Agent {
    public event Action<Brain> onEpisodeBegin;
    public event Action<Brain> onAction;
    public event Action<Brain> onFall;

    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Gatherer gatherer = default;

    public bool reset = false;

    public Vector3 localPosition {
        get => movement.transform.localPosition;
        set => movement.transform.localPosition = value;
    }

    public override void Initialize() {
        gatherer.onCollect += CollectListener;
    }
    void CollectListener(Interactable interactable) {
    }

    public override void OnEpisodeBegin() {
        reset = false;
        onEpisodeBegin?.Invoke(this);
    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(movement.localVelocity);
        sensor.AddObservation(movement.angularVelocityY);
    }
    static readonly IDictionary<int, float> thrustValues = new Dictionary<int, float>() {
        [0] = -1,
        [1] = 0,
        [2] = 1,
    };
    static readonly IDictionary<int, float> torqueValues = new Dictionary<int, float>() {
        [0] = -1,
        [1] = 0,
        [2] = 1,
    };
    static readonly IDictionary<int, bool> isJumpingValues = new Dictionary<int, bool>() {
        [0] = false,
        [1] = true,
    };
    public override void OnActionReceived(float[] actions) {
        onAction?.Invoke(this);

        movement.thrust = thrustValues[Mathf.RoundToInt(actions[0])];
        movement.torque = torqueValues[Mathf.RoundToInt(actions[1])];
        movement.isJumping = isJumpingValues[Mathf.RoundToInt(actions[2])];

        if (transform.position.y < -1) {
            onFall?.Invoke(this);
        }
        if (reset) {
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actions) {
        actions[0] = Input.GetAxisRaw("Vertical") + 1;
        actions[1] = Input.GetAxisRaw("Horizontal") + 1;
        actions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public void Reset() {
        movement.Reset();
    }
}
