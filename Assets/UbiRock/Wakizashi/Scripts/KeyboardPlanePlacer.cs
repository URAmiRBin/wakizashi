using UnityEngine;
using UbiRock.Wakizashi;

public class KeyboardPlanePlacer : UISlicer {
    [SerializeField] LayerMask _layer;
    [SerializeField] MonoBehaviourSlicer slicer;
    bool _isActive = false;
    GameObject _plane;
    Transform _planeTransform;

    void Awake() {
        _planeTransform = transform.GetChild(0);
        _plane = _planeTransform.gameObject;
    }

    void Update() {
        if (!_isActive) return;
        if (Input.GetMouseButtonDown(0)) {
            FindAndSlice();
        } else {
            _planeTransform.RotateAround(transform.position, transform.forward, Input.mouseScrollDelta.y);
        }
    }


    void FindAndSlice()
    {
        Collider[] hitColliders = Physics.OverlapBox(_planeTransform.position, _planeTransform.localScale / 2, _planeTransform.rotation, _layer);
        slicer.SetSlicePlane(_planeTransform);

        foreach(Collider collider in hitColliders) {
            slicer.meshToSlice = collider.gameObject;
            slicer.Slice();
        }
    }

    public override void Activate(bool value) {
        PlayerController._excludeRotation = value;
        _isActive = value;
        _plane.SetActive(value);
    }
}
