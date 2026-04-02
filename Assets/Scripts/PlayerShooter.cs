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
        if (playerInput.Fire)
        {
            gun.Fire();
        }
    }
}
