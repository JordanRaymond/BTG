using UnityEngine;
using UnityEngine.UI;

namespace FPS.DebugUtils
{
    public class DebugController : MonoBehaviour
    {
        public FpsController player;
        public PlayerParams accellParams;
        public PlayerParams velocityChangeParams;
        public Text forceText;
        public Text stateText;
        public Text infoText;

        private int counter = 2;
        void Start()
        {
            player = FindObjectOfType<FpsController>();
            forceText.text = "Acceleration";
        }

        void Update()
        {
            if (stateText != null) stateText.text = player.currentState.GetName();

            if (infoText != null && Input.GetKeyDown(KeyCode.I))
            {
                infoText.enabled = !infoText.enabled;
            }
        }
    }
}

