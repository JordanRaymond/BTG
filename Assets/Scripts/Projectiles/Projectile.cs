using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour, IProjectile
{
    public float moveSpeed = 2; // Number of meter moved for each bpm: Meter/TimeBtw2Bpm
    public bool haveLifeTime = true;
    public float lifeTimeInSec = 5;
    public AudioClip fireSound;
    public AudioClip movingSound;
    private Transform target;

    #region object refs
    private AudioSource audioSource;
    private MasterTimeLine masterTimeLine;
    #endregion
    private float currentBpm = 60;
    private float currentMoveSpeed = 0;
    private float spawnTime = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        OnSpawn();
    }

    public void Init(Transform p_target, MasterTimeLine p_masterTimeLine)
    {
        SetTarget(p_target);
        masterTimeLine = p_masterTimeLine;

        currentBpm = masterTimeLine.Bpm;
        currentMoveSpeed = CalculateMovespeed();
    }

    public void SetTarget(Transform p_target)
    {
        target = p_target;
    }

    void Update()
    {
        Move();
        if (haveLifeTime && Time.time - spawnTime >= lifeTimeInSec)
        {
            OnDestroy();
        }
    }

    protected virtual void Move()
    {
        if (!target)
            return;

        Vector3 dir = (target.position - transform.position).normalized;

        // Then add the direction * the speed to the current position:
        transform.position += dir * currentMoveSpeed * Time.deltaTime;
    }

    private void OnSpawn()
    {
        spawnTime = Time.time;
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
        audioSource.clip = movingSound;
        audioSource.Play();
    }

    public void PlayFireSound(AudioSource audioSource)
    {

    }

    private void OnColision()
    {
        OnDestroy();
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void OnPlayerHit()
    {

    }

    private float CalculateMovespeed()
    {
        return moveSpeed / masterTimeLine.BeatDuration;
    }

}
