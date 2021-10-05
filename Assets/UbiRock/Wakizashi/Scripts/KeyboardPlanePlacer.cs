using UnityEngine;
using UbiRock.Wakizashi;

public class KeyboardPlanePlacer : UISlicer {
    [SerializeField] LayerMask _layer;
    [SerializeField] MonoBehaviourSlicer slicer;
    bool _isActive = false;
    GameObject _plane;
    Transform _planeTransform;
    Vector3 _size;

    void Awake() {
        _planeTransform = transform.GetChild(0);
        _plane = _planeTransform.gameObject;
        _size = _plane.GetComponent<BoxCollider>().size;
    }

    void Update() {
        if (!_isActive) return;
        if (Input.GetMouseButtonDown(0)) {
            FindAndSlice();
        } else {
            _planeTransform.RotateAround(transform.position, transform.forward, Input.mouseScrollDelta.y);
        }
    }

    bool m_Started;
    void FindAndSlice()
    {
        m_Started = true;
        Collider[] hitColliders = Physics.OverlapBox(_planeTransform.position, _size, _planeTransform.rotation, _layer);
        slicer.SetSlicePlane(_planeTransform);

        foreach(Collider collider in hitColliders) {
            Debug.Log(collider.gameObject.name);
            slicer.meshToSlice = collider.gameObject;
            slicer.Slice();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(_planeTransform.position, _size);
    }

    public override void Activate(bool value) {
        PlayerController._excludeRotation = value;
        _isActive = value;
        _plane.SetActive(value);
    }
}
