using UnityEngine;

public class SliceController : MonoBehaviour {
    [SerializeField] UISlicer[] _slicers;
    [SerializeField] Scroller _scroller;
    int _currentSlicerIndex = 0;
    bool _isSlicing = false;

    void Awake() => InputManager.onSetInputLock += SetSliceUnlock;

    void Update() {
        if (_isSlicing) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                ChooseNextSlicer();
            }
        }
    }

    void SetSliceUnlock(bool unlock) {
        _isSlicing = unlock;
        _scroller.gameObject.SetActive(unlock);
        _slicers[_currentSlicerIndex].Activate(unlock);
    }

    public void ChooseNextSlicer() {
        if (_scroller.Scroll()) {
            _slicers[_currentSlicerIndex].Activate(false);
            _slicers[_currentSlicerIndex = _scroller.NextItemIndex].Activate(true);
        }
    }
}
