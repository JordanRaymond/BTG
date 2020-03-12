using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnBeat : MonoBehaviour {

    public Animator animator;
    public string triggerName = "Animate";

	void Start () {
        if (animator == null) {
            animator = GetComponent<Animator>();
        }
	}
	
    public void TriggerAnimation(double nextTick) {
        animator.SetTrigger(triggerName);
    }
}
