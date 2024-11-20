using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float verticalInput;
    public float horizontalInput;
    public bool handbrake;
    public bool nitrous;

    private void FixedUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
        if (Input.GetKey(KeyCode.LeftAlt)) nitrous = true; else nitrous = false;
    }
}
