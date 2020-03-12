using UnityEngine;

namespace WOS.DebugUtils
{
    public class OnCollisionEnterTrigger : MonoBehaviour
    {
        Rigidbody rigid;
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }
        void OnCollisionEnter(Collision collision)
        {
            rigid.isKinematic = false;
        }
    }
}

