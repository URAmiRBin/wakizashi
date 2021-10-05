using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardPlanePlacer : MonoBehaviour, UISlicer
{
    bool _isActive = false;
    public void SetActive(bool value) => _isActive = true;
}
