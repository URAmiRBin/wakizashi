using UnityEngine;

public class SliceController : MonoBehaviour {
    UISlicer[] _slicers;
    int _currentSlicerIndex = 0;

    void ChooseNextSlicer() => _currentSlicerIndex = _currentSlicerIndex >= _slicers.Length - 1 ? 0 : _currentSlicerIndex + 1;
    void ChoosePreviousSlicer() => _currentSlicerIndex = _currentSlicerIndex <= 0 ? _slicers.Length - 1 : _currentSlicerIndex - 1;

    public void StartSlice() {
        _slicers[_currentSlicerIndex].SetActive(true);
    }
}
