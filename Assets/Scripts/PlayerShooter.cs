using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private Gun gun;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (playerInput.Fire)
        {
            gun.Fire();
        }
    }
}
