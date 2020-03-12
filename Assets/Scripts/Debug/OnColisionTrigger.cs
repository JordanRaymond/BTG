using UnityEngine;

namespace FPS.DebugUtils
{
    public class OnColisionTrigger : MonoBehaviour
    {
        public float force = 100f;
        Vector3 up;
        void Start()
        {
            up = transform.up;
        }
        void OnCollisionEnter(Collision collision)
        {
            FpsController player = collision.gameObject.GetComponent<FpsController>();
            if (player != null)
            {
                player.defaultPlayerParams.rigidBody.AddForce(up * force, ForceMode.Impulse);
            }
        }
    }
}

