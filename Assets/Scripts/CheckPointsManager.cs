using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace FPS
{
    public class CheckPointsManager : MonoBehaviour
    {
        public List<Transform> checkpoints = new List<Transform>();
        public Text checkpointText;

        private FpsController player;
        private int indexCurrentCheckpoint = 0;

        void Start()
        {
            player = FindObjectOfType<FpsController>();
            CreateCheckpoints();
        }

        // Update is called once per frame
        void Update()
        {
            if (checkpoints.Count != 0)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    player.transform.position = checkpoints[indexCurrentCheckpoint].position;
                    player.defaultPlayerParams.rigidBody.velocity = Vector3.zero;
                }
                if (Input.GetKeyDown(KeyCode.Minus))
                {
                    if (indexCurrentCheckpoint == 0) indexCurrentCheckpoint = checkpoints.Count - 1;
                    else indexCurrentCheckpoint -= 1;
                }
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    if (indexCurrentCheckpoint == checkpoints.Count - 1) indexCurrentCheckpoint = 0;
                    else indexCurrentCheckpoint += 1;
                }

                if (checkpointText != null)
                    checkpointText.text = "Checkpoint " + indexCurrentCheckpoint;
            }
        }

        public void SetCurrentCheckpoint(int index)
        {
            indexCurrentCheckpoint = index;
        }

        private void CreateCheckpoints()
        {
            for (int i = 0; i < checkpoints.Count; i++)
            {
                CheckPoint checkPointComponent = checkpoints[i].gameObject.AddComponent(typeof(CheckPoint)) as CheckPoint;
                checkPointComponent.SetIndex(i);
            }
        }
    }
}

