using System;
using UnityEngine;

[SelectionBase]
public class Movement : MonoBehaviour {
    public event Action<Movement> onFall;

    [SerializeField]
    Rigidbody attachedRigidbody = default;

    [Header("Movement")]
    [SerializeField, Range(0, 1000)]
    float maxForwardSpeedGrounded = 10;
    [SerializeField, Range(0, 1)]
    float forwardLerpGrounded = 1;
    [SerializeField, Range(0, 1000)]
    float maxForwardSpeedAirborne = 5;
    [SerializeField, Range(0, 1)]
    float forwardLerpAirborne = 1;

    [Header("Turning")]
    [SerializeField, Range(0, 1000)]
    float maxTurnSpeedGrounded = 400;
    [SerializeField, Range(0, 1)]
    float turnLerpGrounded = 1;
    [SerializeField, Range(0, 1000)]
    float maxTurnSpeedAirborne = 200;
    [SerializeField, Range(0, 1)]
    float turnLerpAirborne = 1;

    [Header("Jumping")]
    [SerializeField, Range(0, 10)]
    float jumpHeight = 1;
    float jumpForce => Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

    [SerializeField, Range(-10, 10)]
    float deathzoneY = -1;

    public float thrust {
        get => thrustCache;
        set => thrustCache = Mathf.Clamp(value, -0.5f, 1);
    }
    float thrustCache;

    public float torque {
        get => torqueCache;
        set => torqueCache = Mathf.Clamp(value, -1, 1);
    }
    float torqueCache;

    [Header("Debug variables")]
    public bool isJumping;
    public bool isGrounded;

    public Vector3 localVelocity;
    public float angularVelocityY;

    public Vector3 startingPosition;
    public Quaternion startingRotation;

    void Awake() {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    void Start() {
        Reset();
    }

    public void Reset() {
        thrust = 0;
        torque = 0;
        isJumping = false;

        transform.position = lastPosition = startingPosition;
        transform.rotation = startingRotation;

        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;

        thurstVelocity = Vector3.zero;
        torqueVelocity = Vector3.zero;
    }

    Vector3 thurstVelocity = Vector3.zero;
    Vector3 torqueVelocity = Vector3.zero;

    Vector3 lastPosition = Vector3.zero;
    Vector3 observedVelocity = Vector3.zero;
    void FixedUpdate() {
        if (transform.position.y < deathzoneY) {
            onFall?.Invoke(this);
            Reset();
            return;
        }

        observedVelocity = (transform.position - lastPosition) / Time.deltaTime;
        localVelocity = transform.InverseTransformDirection(observedVelocity) / maxForwardSpeedGrounded;
        lastPosition = transform.position;

        angularVelocityY = torqueVelocity.y / maxTurnSpeedGrounded;

        if (isGrounded) {
            thurstVelocity = Vector3.Lerp(thurstVelocity, transform.forward * thrust * maxForwardSpeedGrounded, forwardLerpGrounded);
            torqueVelocity = Vector3.Lerp(torqueVelocity, transform.up * torque * maxTurnSpeedGrounded, turnLerpGrounded);
            if (isJumping) {
                isJumping = false;
                attachedRigidbody.velocity = new Vector3(attachedRigidbody.velocity.x, jumpForce, attachedRigidbody.velocity.z);
            }
        } else {
            thurstVelocity = Vector3.Lerp(thurstVelocity, transform.forward * thrust * maxForwardSpeedAirborne, forwardLerpAirborne);
            torqueVelocity = Vector3.Lerp(torqueVelocity, transform.up * torque * maxTurnSpeedAirborne, turnLerpAirborne);
        }

        attachedRigidbody.MovePosition(attachedRigidbody.position + (thurstVelocity * Time.deltaTime));
        attachedRigidbody.MoveRotation(attachedRigidbody.rotation * Quaternion.Euler(torqueVelocity * Time.deltaTime));
    }
}
