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
        else if (Input.GetKeyDown(KeyCode.A)) {
            KeyScreenCaster.PlayAnimationWithString("A");
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            KeyScreenCaster.PlayAnimationWithString("S");
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            KeyScreenCaster.PlayAnimationWithString("D");
        }
        else if (Input.GetKeyDown(KeyCode.R)) {
            KeyScreenCaster.PlayAnimationWithString("R");
            TrashCan.Recycle();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            onSetInputLock?.Invoke(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            onSetInputLock?.Invoke(true);
        }
    }
}
