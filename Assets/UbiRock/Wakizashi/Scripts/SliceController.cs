using UnityEngine;

public class SliceController : MonoBehaviour {
    [SerializeField] UISlicer[] _slicers;
    [SerializeField] Scroller _scroller;
    int _currentSlicerIndex = 0;
    bool _isSlicing = false;

    void Awake() => _scroller.gameObject.SetActive(false);

    void OnEnable() => InputManager.onSetInputLock += SetSliceUnlock;

    void Update() {
        if (_isSlicing) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                ChooseNextSlicer();
            }
        }
    }

    void SetSliceUnlock(bool unlock) {
        _isSlicing = unlock;
        if (unlock) {
            _scroller.gameObject.SetActive(true);
            _slicers[_currentSlicerIndex].Activate(true);
        } else {
            _scroller.gameObject.SetActive(false);
            _slicers[_currentSlicerIndex].Activate(false);
        }
    }

    public void ChooseNextSlicer() {
        _slicers[_currentSlicerIndex].Activate(false);
        _slicers[_currentSlicerIndex = _scroller.NextItemIndex].Activate(true);
        _scroller.Scroll();
    }
}
