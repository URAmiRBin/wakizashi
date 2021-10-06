using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UbiRock.Utils;

public class ObjectCreator : MonoBehaviour {
    [SerializeField] Scroller _scroller;
    [SerializeField] ObjectOptions _options;
    [SerializeField] Transform garbage;
    int _currentShapeIndex = 0;
    GameObject[] shapeResources = new GameObject[5];
    List<MeshRenderer> shapes = new List<MeshRenderer>();
    [SerializeField] Material fillMaterial, emptyMaterial;

    bool _isMoving;
    bool IsCreating => _currentShapeIndex != 0;
    
    void Awake() {
        InputManager.onSetInputLock += SetMoving;
        LoadResources();
        InstantiateSamples();
    }

    void LoadResources() {
        shapeResources[1] = Resources.Load<GameObject>("Plane");
        shapeResources[2] = Resources.Load<GameObject>("Cube");
        shapeResources[3] = Resources.Load<GameObject>("Sphere");
        shapeResources[4] = Resources.Load<GameObject>("Cylinder");
    }

    void InstantiateSamples() {
        foreach(GameObject go in shapeResources) {
            if (go == null) shapes.Add(null);
            else if (go.name == "Plane") {
                shapes.Add(Instantiate(go, transform.position + transform.forward * 10, Quaternion.Euler(-90, 0, 0), transform).GetComponent<MeshRenderer>());
                shapes[shapes.Count - 1].enabled = false;
            }
            else {
                shapes.Add(Instantiate(go, transform.position + transform.forward * 10, Quaternion.identity, transform).GetComponent<MeshRenderer>());
                shapes[shapes.Count - 1].enabled = false;
            }
        }
    }

    void Update() {
        if (_isMoving) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                ChooseNextShape();
            } else if (Input.GetMouseButtonDown(0)) {
                MakeObject();
            }
        }

        if (IsCreating) {
            if (Input.GetKeyDown(KeyCode.C)) {
                _options.SwitchPhysics();
            } else if (Input.GetKeyDown(KeyCode.V)) {
                _options.SwitchFill();
            } else if (Input.GetKeyDown(KeyCode.Escape)) {
                if (_currentShapeIndex != 0) shapes[_currentShapeIndex].enabled = false;
                _currentShapeIndex = 0;
                _options.SetDisplay(false);
                _scroller.Reset();
            }
        }
    }

    void SetMoving(bool value) {
        if ((_isMoving && !value) || (!_isMoving && value)) return;
        _isMoving = !value;
        _scroller.Reset();
        if (_currentShapeIndex != 0) shapes[_currentShapeIndex].enabled = false;
        _currentShapeIndex = 0;
        _options.SetDisplay(false);
    }

    void ChooseNextShape() {
        if (_currentShapeIndex != 0) shapes[_currentShapeIndex].enabled = false;
        _currentShapeIndex = _scroller.NextItemIndex;
        if (_currentShapeIndex != 0) shapes[_currentShapeIndex].enabled = true;
        _scroller.Scroll();
        _options.SetDisplay(IsCreating);
    }

    void MakeObject() {
        if (shapes[_currentShapeIndex] == null) return;
        GameObject go = Instantiate(shapes[_currentShapeIndex].gameObject, shapes[_currentShapeIndex].transform.position, shapes[_currentShapeIndex].transform.rotation, garbage);
        Tweener.Instance.ScalePop(go.transform, 0.95f, 0.15f, EaseType.Cubic);
        Sliceable sliceable = go.GetComponent<Sliceable>();
        var (physics, fill) = _options.GetStatus();
        sliceable.SetOptions(physics, fill);
        go.layer = 6;
        go.GetComponent<Renderer>().material = fill ? fillMaterial : emptyMaterial;
        go.GetComponent<Collider>().enabled = true;
    }
}
