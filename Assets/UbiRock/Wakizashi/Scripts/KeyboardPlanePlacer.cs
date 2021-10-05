using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardPlanePlacer : UISlicer
{
    bool _isActive = false;
    public override void Activate(bool value) => _isActive = true;
}
