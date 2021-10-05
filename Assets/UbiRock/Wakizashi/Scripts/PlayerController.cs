using UnityEngine;
public class PlayerController : MonoBehaviour
{

    public float XSensitivity, YSensitivity;
    public int minY, maxY;

    [Header("Movement")]
    public float walkSpeed;
    bool  _isInputLock;
    public static bool _excludeRotation;

    float verticalRotation, horizontalRotation;

    void Awake() => InputManager.onSetInputLock += SetInputLock;

    void SetInputLock(bool value) => _isInputLock = value;

    void Update()
    {
        if (_isInputLock && !_excludeRotation) return;

        float mouseX = Input.GetAxis("Mouse X"), mouseY = Input.GetAxis("Mouse Y");

        verticalRotation += mouseY * YSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minY, maxY);
        horizontalRotation += mouseX * XSensitivity;

        transform.eulerAngles = new Vector3(-verticalRotation, horizontalRotation, 0);

        if (_isInputLock) return;

        float horizontal = Input.GetAxis("Horizontal"), forward = Input.GetAxis("Vertical");

        transform.position += transform.right * horizontal * walkSpeed;
        transform.position +=  new Vector3(transform.forward.x, 0, transform.forward.z).normalized * forward * walkSpeed;

        if (Input.GetMouseButtonDown(0)) PhysicsImpact();
    }

    void PhysicsImpact() {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 50)) {
            Vector3 position = hit.point;
            Collider[] objects = UnityEngine.Physics.OverlapSphere(position, 10f);
            foreach (Collider h in objects)
            {
                Rigidbody r = h.GetComponent<Rigidbody>();
                if (r != null) r.AddExplosionForce(250, position, 50);
            }
        }
    }
}