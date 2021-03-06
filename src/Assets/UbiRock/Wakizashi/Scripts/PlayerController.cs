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

    void SetInputLock(bool value) {
        _isInputLock = value;
        if (value) ContextSensitiveHelper.Instance.RemoveHelp(ContextSensitiveHelper.Mode.Move);
        else ContextSensitiveHelper.Instance.AddHelp(ContextSensitiveHelper.Mode.Move);
    }

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
        if (transform.position.x > 49) transform.position = new Vector3(49f, transform.position.y, transform.position.z);
        if (transform.position.x < -49) transform.position = new Vector3(-49f, transform.position.y, transform.position.z);
        if (transform.position.z > 49) transform.position = new Vector3(transform.position.x, transform.position.y, 49f);
        if (transform.position.z < -49) transform.position = new Vector3(transform.position.x, transform.position.y, -49f);

        if (Input.GetMouseButtonDown(0)) PhysicsImpact();
    }

    void PhysicsImpact() {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
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