using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float shotDistance = 20f;
    private float interval = 0.1f;
    private float lastShotTime = 0f;

    [SerializeField]
    private Transform shotStart;    // 총구 위치

    [SerializeField]
    private ParticleSystem shotParticle;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public AudioClip shotClip;

    private Coroutine coShot;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }
    private void Start()
    {
        
    }

    public void Fire()
    {
        if (lastShotTime + interval < Time.time)
        {
            lastShotTime = Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        Vector3 hitPosition = Vector3.zero;

        Ray ray = new Ray(shotStart.position, shotStart.forward);
        if (Physics.Raycast(ray, out RaycastHit hit ,shotDistance))
        {
            hitPosition = hit.point;

            var target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(20f, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = shotStart.position + shotStart.forward * shotDistance;
        }

        if (coShot != null)                                                 // 실행중인 코루틴이 있으면 멈춤 
        {
            StopCoroutine(coShot);
            coShot = null;
        }
        coShot = StartCoroutine(CoShotEffect(hitPosition));

        //if (lastShotTime + interval < Time.deltaTime)
        //{
        //    lastShotTime = Time.time;
        //}
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        shotParticle.Play();

        SoundManager.instance.PlaySFX(shotClip);
        lineRenderer.SetPosition(0, shotStart.position);
        lineRenderer.SetPosition(1, hitPosition);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;

        coShot = null;
    }
}