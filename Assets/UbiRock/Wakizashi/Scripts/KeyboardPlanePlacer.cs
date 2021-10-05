using UnityEngine;

public class KeyboardPlanePlacer : UISlicer {
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
            // TODO: CUT
        } else {
            _planeTransform.RotateAround(transform.position, transform.forward, Input.mouseScrollDelta.y);
        }
    }

    public override void Activate(bool value) {
        _isActive = value;
        _plane.SetActive(value);
    }
}
