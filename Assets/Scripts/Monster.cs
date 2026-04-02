using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    [SerializeField]
    private Transform target;
    [SerializeField]
    private ParticleSystem takeDamageEffect;
    [SerializeField]
    private LayerMask targetLayer;
    private Collider monsterCollider;
    private AudioSource monsterAudioSource;
    private NavMeshAgent agent;
    private Animator zombieAnimator;

    private AudioClip hitClip;
    private AudioClip deathClip;

    public float traceDistance = 10f;
    public float attackInterval = 1f;
    public float attackDistance = 1f;
    private float lastAttackTime = 0f;
    private float damage;

    private Status currentStatus;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            Debug.Log(currentStatus);

            switch (currentStatus)
            {
                case Status.Idle:
                    zombieAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    zombieAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    zombieAnimator.SetTrigger("Die");
                    agent.isStopped = true;
                    break;
            }
        }
    }

    void Awake()
    {
        monsterCollider = GetComponent<Collider>();
        monsterAudioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        CurrentStatus = Status.Idle;
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(target.position, transform.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
        // 주변 탐색, 찾으면 Trace로 전환
    }
    private void UpdateTrace()
    {
        // 타깃이 사라졌거나 탐색 범위를 벗어나면 Idle로 전환

        if (target != null && Vector3.Distance(target.position, transform.position) <= attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        if (target == null)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);
    }
    private void UpdateAttack()
    {
        // 타깃이 공격범위 내에 있으면 데미지를 입힘
        if (target == null || Vector3.Distance(target.position, transform.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;

        transform.LookAt(lookAt);

        if (Time.time > lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            Debug.Log("공격");
        }
    }
    private void UpdateDie()
    {

    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if (colliders.Length == 0)
            return null;

        Array.Sort(colliders);
        Collider target = colliders.First();
        return target.transform;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        //monsterAudioSource.PlayOneShot(hitClip);
        takeDamageEffect.transform.position = hitPoint;
        takeDamageEffect.transform.forward = hitNormal;
        takeDamageEffect.Play();
    }

    public override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        CurrentStatus = Status.Die;
    }
}
