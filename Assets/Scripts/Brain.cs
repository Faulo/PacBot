using System;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

[SelectionBase]
public class Brain : Agent {
    public event Action<Brain> onEpisodeBegin;
    public event Action<Brain> onAction;

    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Arena arena = default;
    [SerializeField, Range(0, 10)]
    int allowedStepsPerTile = 0;

    void Awake() {
        if (!movement) {
            movement = GetComponentInParent<Movement>();
        }
        if (!arena) {
            arena = GetComponentInParent<Arena>();
        }
        MaxStep = arena.size.x * arena.size.y * allowedStepsPerTile;
    }

    public override void OnEpisodeBegin() {
        movement.startingPosition = arena.PositionOnRandomTile();
        movement.startingRotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360));

        movement.Reset();
        arena.Reset();
        onEpisodeBegin?.Invoke(this);
    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(movement.localVelocity);
        sensor.AddObservation(movement.angularVelocityY);
        sensor.AddObservation(movement.isGrounded);
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
    }
    public override void Heuristic(float[] actions) {
        actions[0] = Input.GetAxisRaw("Vertical") + 1;
        actions[1] = Input.GetAxisRaw("Horizontal") + 1;
        actions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
