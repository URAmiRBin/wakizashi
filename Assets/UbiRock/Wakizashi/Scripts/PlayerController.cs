using UnityEngine;
public class PlayerController : MonoBehaviour
{

    public float XSensitivity, YSensitivity;
    public int minY, maxY;

    [Header("Movement")]
    public float walkSpeed;

    float verticalRotation, horizontalRotation;

    bool isRotationLocked = false;



    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X"), mouseY = Input.GetAxis("Mouse Y");

        verticalRotation += mouseY * YSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minY, maxY);
        horizontalRotation += mouseX * XSensitivity;

        if (!isRotationLocked) transform.eulerAngles = new Vector3(-verticalRotation, horizontalRotation, 0);

        float horizontal = Input.GetAxis("Horizontal"), forward = Input.GetAxis("Vertical");

        transform.position += transform.right * horizontal * walkSpeed;
        transform.position +=  new Vector3(transform.forward.x, 0, transform.forward.z).normalized * forward * walkSpeed;
    }
}