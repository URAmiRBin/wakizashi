using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour {
    bool _physics, _fill;
    Mesh _mesh;

    public bool Physics { get => _physics; }
    public bool Fill { get => _fill; }

    public Mesh Mesh { get => _mesh; }

    void Awake() => _mesh = GetComponent<MeshFilter>().sharedMesh;

    public void SetOptions(bool physics, bool fill) {
        _fill = fill;
        _physics = physics;
    }
}
