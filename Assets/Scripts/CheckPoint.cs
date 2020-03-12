using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;

namespace Core
{
    public class CheckPoint : MonoBehaviour
    {
        private int checkpointIndex = 0;
        private CheckPointsManager checkpointController;

        void Start()
        {
            checkpointController = FindObjectOfType<CheckPointsManager>();
        }

        void OnTriggerEnter(Collider collider)
        {
            FpsController player = collider.gameObject.GetComponent<FpsController>();
            if (player != null)
            {
                checkpointController.SetCurrentCheckpoint(checkpointIndex);
            }
        }

        public void SetIndex(int index)
        {
            checkpointIndex = index;
        }
    }
}

