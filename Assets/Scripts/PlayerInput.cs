using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string upDown = "Vertical";
    public static readonly string leftRight = "Horizontal";
    public static readonly string fireButton = "Fire1";

    public float UpDown { get; private set; }
    public float LeftDonw { get; private set; }
    public bool Fire { get; private set; }


    private void Update()
    {
        UpDown = Input.GetAxis(upDown);
        LeftDonw = Input.GetAxis(leftRight);
        Fire = Input.GetButton(fireButton);
    }
}
