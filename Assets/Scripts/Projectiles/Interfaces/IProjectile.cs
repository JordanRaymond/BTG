using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Init(Transform target, MasterTimeLine masterTimeLine);
    void SetTarget(Transform target);
    void PlayFireSound(AudioSource audioSource);
}
