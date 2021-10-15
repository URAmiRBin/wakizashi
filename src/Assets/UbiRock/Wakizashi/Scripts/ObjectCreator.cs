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
            } else if (Input.GetKeyDown(KeyCode.F)) {
                MakeObject();
            }
        }

        if (IsCreating) {
            if (Input.GetKeyDown(KeyCode.X)) {
                _options.SwitchPhysics();
            } else if (Input.GetKeyDown(KeyCode.V)) {
                _options.SwitchFill();
            } else if(Input.GetKey(KeyCode.E)) {
                shapes[_currentShapeIndex].transform.localScale += Vector3.right * .01f;
            } else if(Input.GetKey(KeyCode.Q)) {
                shapes[_currentShapeIndex].transform.localScale -= Vector3.right * .01f;
            } else if(Input.GetKey(KeyCode.C)) {
                if (_currentShapeIndex == 1) shapes[_currentShapeIndex].transform.localScale += Vector3.forward * .01f;
                else shapes[_currentShapeIndex].transform.localScale += Vector3.up * .01f;
            } else if(Input.GetKey(KeyCode.Z)) {
                if (_currentShapeIndex == 1) shapes[_currentShapeIndex].transform.localScale -= Vector3.forward * .01f;
                else shapes[_currentShapeIndex].transform.localScale -= Vector3.up * .01f;
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                if (_currentShapeIndex != 0) shapes[_currentShapeIndex].enabled = false;
                _currentShapeIndex = 0;
                ContextSensitiveHelper.Instance.RemoveHelp(ContextSensitiveHelper.Mode.Creation);
                _options.SetDisplay(false);
                _scroller.Reset();
            }
        }
    }

    void SetMoving(bool value) {
        if ((_isMoving && !value) || (!_isMoving && value)) return;
        _isMoving = !value;
        _scroller.Reset();
        if (IsCreating) shapes[_currentShapeIndex].enabled = false;
        _currentShapeIndex = 0;
        ContextSensitiveHelper.Instance.RemoveHelp(ContextSensitiveHelper.Mode.Creation);
        _options.SetDisplay(false);
    }

    void ChooseNextShape() {
        if (IsCreating) shapes[_currentShapeIndex].enabled = false;
        _currentShapeIndex = _scroller.NextItemIndex;
        if (IsCreating) {
            shapes[_currentShapeIndex].enabled = true;
            ContextSensitiveHelper.Instance.AddHelp(ContextSensitiveHelper.Mode.Creation);
        } else {
            ContextSensitiveHelper.Instance.RemoveHelp(ContextSensitiveHelper.Mode.Creation);
        }
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

        if (!fill) {
            Mesh result = new Mesh();
            Mesh mesh = go.GetComponent<MeshFilter>().mesh;
            int vertexCount = mesh.vertices.Length;
            int[] tris = mesh.GetTriangles(0);
            int[] indices = new int[tris.Length * 2];
            Vector3[] vertecies = new Vector3[2 * vertexCount];
            Vector3[] normals = new Vector3[2 * vertexCount];
            Vector2[] uvs = new Vector2[2 * vertexCount];
            
            for(int i = 0; i < vertexCount; i += 1) {
                vertecies[i] = mesh.vertices[i];
                normals[i] = mesh.normals[i];
                uvs[i] = mesh.uv[i];
            }

            for(int i = 0; i < tris.Length; i += 3) {
                indices[i] = tris[i];
                indices[i + 1] = tris[i + 1];
                indices[i + 2] = tris[i + 2];
            }

            for(int i = 0; i < tris.Length; i += 3) {
                indices[i + tris.Length] = tris[i + 2];
                indices[i + 1 + tris.Length] = tris[i + 1];
                indices[i + 2 + tris.Length] = tris[i];
            }

            for(int i = 0; i < vertexCount; i += 1) {
                vertecies[i + vertexCount] = mesh.vertices[i];
                normals[i + vertexCount] = -mesh.normals[i];
                uvs[i + vertexCount] = mesh.uv[i];
            }

            result.vertices = vertecies;
            result.normals = normals;
            result.uv = uvs;
            result.SetTriangles(indices, 0, false);

            go.GetComponent<MeshFilter>().mesh = result;
        }
    }
}
