using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField]
    Rigidbody attachedRigidbody = default;

    [Header("Movement")]
    [SerializeField, Range(0, 1000)]
    float maxForwardSpeed = 100;
    [SerializeField, Range(0, 1)]
    float forwardLerpGrounded = 1;
    [SerializeField, Range(0, 1)]
    float forwardLerpAirborne = 1;
    [SerializeField, Range(0, 1000)]
    float maxTurnSpeed = 100;
    [SerializeField, Range(0, 1)]
    float turnLerpGrounded = 1;
    [SerializeField, Range(0, 1)]
    float turnLerpAirborne = 1;

    [SerializeField, Range(0, 10)]
    float jumpHeight = 1;
    float jumpForce => Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

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

    [Header("Debug")]
    public bool isJumping;
    public bool isGrounded;

    public Vector3 localVelocity => attachedRigidbody.velocity;
    public float angularVelocity => attachedRigidbody.angularVelocity.y;

    Vector3 startingPosition;
    Quaternion startingRotation;

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
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;
    }

    Vector3 thurstVelocity = Vector3.zero;
    Vector3 torqueVelocity = Vector3.zero;
    void FixedUpdate() {
        if (isGrounded) {
            thurstVelocity = Vector3.Lerp(thurstVelocity, transform.forward * thrust * maxForwardSpeed, forwardLerpGrounded);
            torqueVelocity = Vector3.Lerp(torqueVelocity, transform.up * torque * maxTurnSpeed, turnLerpGrounded);
            if (isJumping) {
                attachedRigidbody.velocity = new Vector3(attachedRigidbody.velocity.x, jumpForce, attachedRigidbody.velocity.z);
            }
        } else {
            thurstVelocity = Vector3.Lerp(thurstVelocity, transform.forward * thrust * maxForwardSpeed, forwardLerpAirborne);
            torqueVelocity = Vector3.Lerp(torqueVelocity, transform.up * torque * maxTurnSpeed, turnLerpAirborne);
        }

        attachedRigidbody.MovePosition(attachedRigidbody.position + (thurstVelocity * Time.deltaTime));
        attachedRigidbody.MoveRotation(attachedRigidbody.rotation * Quaternion.Euler(torqueVelocity * Time.deltaTime));

        attachedRigidbody.angularVelocity = Vector3.zero;
    }
}
