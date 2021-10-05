using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour {
    [SerializeField] Scroller _scroller;
    int _currentShapeIndex = 0;
    GameObject[] shapeResources = new GameObject[5];
    List<GameObject> shapes = new List<GameObject>();
    [SerializeField] Material material;

    bool _isMoving;
    
    void Awake() {
        InputManager.onSetInputLock = SetMoving;
        LoadResources();
        InstantiateSamples();
    }

    void LoadResources() {
        shapeResources[1] = Resources.Load<GameObject>("Cube");
        shapeResources[2] = Resources.Load<GameObject>("Cube");
        shapeResources[3] = Resources.Load<GameObject>("Sphere");
        shapeResources[4] = Resources.Load<GameObject>("Cylinder");
    }

    void InstantiateSamples() {
        foreach(GameObject go in shapeResources) {
            if (go == null) shapes.Add(null);
            else {
                shapes.Add(Instantiate(go, transform.position + transform.forward * 10, Quaternion.identity, transform));
                shapes[shapes.Count - 1].SetActive(false);
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
    }

    void SetMoving(bool value) {
        _isMoving = !value;
        _scroller.gameObject.SetActive(!value);
    }

    void ChooseNextShape() {
        shapes[_currentShapeIndex]?.SetActive(false);
        shapes[_currentShapeIndex = _scroller.NextItemIndex]?.SetActive(true);
        _scroller.Scroll();
    }

    void MakeObject() {
        if (shapes[_currentShapeIndex] == null) return;
        GameObject go = Instantiate(shapes[_currentShapeIndex], shapes[_currentShapeIndex].transform.position, shapes[_currentShapeIndex].transform.rotation);
        go.layer = 6;
        go.GetComponent<Renderer>().material = material;
    }
}
