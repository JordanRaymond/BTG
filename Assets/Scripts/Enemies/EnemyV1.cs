using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyV1 : MonoBehaviour
{
    [Header("Attack params")]
    public float minDistanceToAttack = 5;
    public double chargeTimeInBpm = 3;
    public List<double> pattern = new List<double>();
    public BulletProjectile projectile;
    public Transform projectileSpawnPoint;
    [Header("Refs")]
    public MasterTimeLine masterTimeLine;
    [SerializeField] private AudioSource audioSource;
    public AudioClip fireSound;

    [Header("Infos")]
    [SerializeField] private double targetAttackTimeInBeat = 0;
    [SerializeField] private int currentPatternIndex = 0;

    private bool isAttacking = false;
    private bool isCharging = false;
    private FPS.FpsController player;

    void Start()
    {
        player = FindObjectOfType<FPS.FpsController>();
        audioSource = GetComponent<AudioSource>();
        if (!masterTimeLine)
        {
            masterTimeLine = FindObjectOfType<MasterTimeLine>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (player.transform.position - transform.position).magnitude;

        if (distance <= minDistanceToAttack)
        {
            HandleAttack();
        }
    }

    void HandleAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            targetAttackTimeInBeat = Mathf.Ceil((float)masterTimeLine.MasterPositionInBeat) + pattern[currentPatternIndex];
        }

        if (!isCharging)
        {
            Attack();
        }
        else
        {
            isCharging = false;
            targetAttackTimeInBeat = Mathf.Ceil((float)masterTimeLine.MasterPositionInBeat) + chargeTimeInBpm;
        }
    }

    // Start attack a nextTickTime
    // TODO WHEN PLAYER GO AWAYS RESET BEAT ELSE IT GO OUT OF SYNC
    void Attack()
    {
        // if it's time to attack
        if (masterTimeLine.MasterPositionInBeat >= targetAttackTimeInBeat)
        {
            IProjectile spwnProjectile = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
            spwnProjectile.Init(player.transform, masterTimeLine);
            //audioSource.PlayOneShot(fireSound);
            currentPatternIndex++;

            // If at the end of the pattern
            if (currentPatternIndex > pattern.Count - 1)
            {
                isCharging = true;
                currentPatternIndex = 0;
            }

            targetAttackTimeInBeat = masterTimeLine.MasterPositionInBeat + pattern[currentPatternIndex];
        }
    }
}
