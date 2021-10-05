using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {
    public static UnityAction<bool> onSetInputLock;

    void Awake() => Cursor.visible = false;

    void Start() => onSetInputLock.Invoke(false);

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
            onSetInputLock?.Invoke(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            onSetInputLock?.Invoke(true);
        }
    }
}
