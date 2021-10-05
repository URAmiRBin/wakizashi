using System;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static Action<bool> onSetInputLock;
    void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            KeyScreenCaster.PlayAnimationWithString("W");
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            KeyScreenCaster.PlayAnimationWithString("A");
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            KeyScreenCaster.PlayAnimationWithString("S");
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            KeyScreenCaster.PlayAnimationWithString("D");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            KeyScreenCaster.PlayAnimationLock(false);
            onSetInputLock?.Invoke(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            KeyScreenCaster.PlayAnimationLock(true);
            onSetInputLock?.Invoke(true);
        }
    }
}
