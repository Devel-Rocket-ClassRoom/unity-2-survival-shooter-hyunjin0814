using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public UiManager uiManager;

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
        uiManager.SetHealthSlide(Health / maxHp);
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    { 
        if (!IsDead)
        {
            SoundManager.instance.PlaySFX(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        uiManager.SetHealthSlide(Health / maxHp);

        uiManager.PlayDamageFlash();

        Debug.Log($"체력: {Health}");
    }

    public override void Die()
    {
        if (IsDead)
            return;

        base.Die();

        SoundManager.instance.PlaySFX(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;

        uiManager.OnPlayerDead();
    }
}
