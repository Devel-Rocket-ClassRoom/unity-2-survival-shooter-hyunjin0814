using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private AudioSource playerAudioSource;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;
    private Animator playerAnimator;

    public AudioClip hitClip;
    public AudioClip deathClip;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAnimator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    { 
        if (!IsDead)
        {
            playerAudioSource.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        Debug.Log($"체력: {Health}");
    }

    public override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        playerAudioSource.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }
}
