using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform player;

    private float cameraX;
    private float cameraZ;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        cameraX = player.transform.position.x;
        cameraZ = player.transform.position.z - 5;

        transform.position = new Vector3(cameraX, 5f, cameraZ);
        transform.LookAt(player.transform.position);
    }
}
