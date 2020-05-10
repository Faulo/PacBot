using UnityEngine;

public class GroundTester : MonoBehaviour {
    [SerializeField]
    Movement movement = default;
    [SerializeField, Range(1, 10)]
    int coyoteFrames = 1;

    int groundedFramesLeft = 0;
    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ground")) {
            groundedFramesLeft = coyoteFrames;
        } else {
            Debug.Log(other.gameObject.tag);
        }
    }

    void FixedUpdate() {
        if (groundedFramesLeft > 0) {
            groundedFramesLeft--;
            movement.isGrounded = true;
        } else {
            movement.isGrounded = false;
        }
    }
}
