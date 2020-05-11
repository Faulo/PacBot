using Cinemachine;
using UnityEngine;

public class FollowBot : MonoBehaviour {
    [SerializeField]
    CinemachineVirtualCamera virtualCamera = default;

    [SerializeField]
    string tagToFollow = "Player";
    void Update() {
        if (!virtualCamera.Follow) {
            var bot = GameObject.FindGameObjectWithTag(tagToFollow);
            if (bot) {
                virtualCamera.Follow = bot.transform;
            }
        }
    }
}
