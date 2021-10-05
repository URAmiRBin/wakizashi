using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOptions : MonoBehaviour {
    Text _physicsText, _fillText;

    [SerializeField] Color _physicsOnColor, _physicsOffColor, _fillOnColor, _fillOffColor;

    bool _physics, _fill;
    
    void Awake() {
        _physicsText = transform.GetChild(1).GetComponent<Text>();
        _fillText = transform.GetChild(3).GetComponent<Text>();
        _physics = _fill = true;
    }

    void Start() {
        _physicsText.color = _physicsOnColor;
        _fillText.color = _fillOnColor;
    }

    public void SwitchPhysics() {
        _physics = !_physics;
        _physicsText.text = _physics ? "ON" : "OFF";
        _physicsText.color = _physics ? _physicsOnColor : _physicsOffColor;
    }

    public void SwitchFill() {
        _fill = !_fill;
        _fillText.text = _fill ? "ON" : "OFF";
        _fillText.color = _fill ? _fillOnColor : _fillOffColor;
    }

    public void SetDisplay(bool value) {
        foreach(Transform child in transform) child.gameObject.SetActive(value);
    }

    public (bool, bool) GetStatus() => (_physics, _fill);
}
