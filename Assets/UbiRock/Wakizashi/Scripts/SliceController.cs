using UnityEngine;

public class SliceController : MonoBehaviour {
    [SerializeField] Scroller _scroller;
    UISlicer[] _slicers;
    int _currentSlicerIndex = 0;

    void ChooseNextSlicer() => _currentSlicerIndex = _currentSlicerIndex >= _slicers.Length - 1 ? 0 : _currentSlicerIndex + 1;
    
    public void StartSlice() {
        _slicers[_currentSlicerIndex].SetActive(true);
    }
}
