using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int hash = Animator.StringToHash("Move");

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public float moveSpeed = 5.0f;
    private bool moving = false;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInput.LeftDonw != 0 || playerInput.UpDown != 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        playerAnimator.SetBool(hash, moving);

        // 캐릭터 회전 왜 되는건지 이해하기
        Ray forward = Camera.main.ScreenPointToRay(Input.mousePosition);                    // 마우스 포지션은 스크린 2D, 3D로 투영한 Ray 생성 
        Plane plane = new Plane(Vector3.up, Vector3.zero);                                  // 위를 바라보는 원점을 지나는 가상의 벽 생성 
        if (plane.Raycast(forward, out float rayLength))                                    // 벽과 마우스 Ray랑 부딪혔을 때 부딪힌 거리 rayLength로 반환
        {
            Vector3 direction = forward.GetPoint(rayLength);

            transform.LookAt(new Vector3(direction.x, transform.position.y, direction.z));
        }

    }

    // 앞뒤는 Z로 좌우는 X로 
    private void FixedUpdate()
    {
        Vector3 move = new Vector3(playerInput.LeftDonw, 0f, playerInput.UpDown);
        playerRigidbody.MovePosition(playerRigidbody.position + move * moveSpeed * Time.deltaTime);
    }
}
