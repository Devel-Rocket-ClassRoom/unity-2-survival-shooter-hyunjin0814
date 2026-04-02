using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkill : MonoBehaviour
{
    private PlayerInput playerInput;

    public float duration = 3f;
    public float cooldown = 10f;
    private float lastUseTime = -10f;

    public GameObject ultimateArea; // Sphere 오브젝트를 직접 연결하세요
    public float skillRange = 5f;   // 구체의 반경

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        ultimateArea.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (playerInput.Ultimate && Time.time >= lastUseTime + cooldown)
        {
            StartCoroutine(UltimateRoutine());
        }
    }

    IEnumerator UltimateRoutine()
    {
        lastUseTime = Time.time;

        // 멈춘 적들을 관리할 임시 리스트
        List<NavMeshAgent> trappedAgents = new List<NavMeshAgent>();

        // 구체 활성화 및 위치 고정
        ultimateArea.SetActive(true);
        ultimateArea.transform.position = transform.position + Vector3.up * 0.5f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            // [핵심] 매 프레임마다 반경 내의 콜라이더를 찾습니다 (Rigidbody 불필요)
            Collider[] targets = Physics.OverlapSphere(transform.position, skillRange);

            foreach (Collider col in targets)
            {
                NavMeshAgent agent = col.GetComponent<NavMeshAgent>();

                // 에이전트가 있고 아직 리스트에 없다면 (새로 들어온 적이라면)
                if (agent != null && !trappedAgents.Contains(agent))
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    trappedAgents.Add(agent);
                }
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 3초 종료 후 모든 적 해제
        foreach (NavMeshAgent agent in trappedAgents)
        {
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }
        }

        ultimateArea.SetActive(false);
    }
}