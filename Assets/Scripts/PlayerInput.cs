using UnityEngine;

public class PlayerInput : MonoBehaviour {
    [SerializeField]
    Movement movement = default;

    [SerializeField]
    string thrustAxis = "Vertical";
    [SerializeField]
    string torqueAxis = "Horizontal";
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    void Update() {
        movement.thrust = Input.GetAxisRaw(thrustAxis);
        movement.torque = Input.GetAxisRaw(torqueAxis);
        movement.isJumping = Input.GetKey(jumpKey);
    }
}
