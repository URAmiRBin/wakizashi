using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {
    [SerializeField] float _doubleTapThreshold = .2f;
    public static UnityAction<bool> onSetInputLock;

    float _lastResetTime;

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
            if (Time.time - _lastResetTime <= _doubleTapThreshold) {
                Debug.Log("QUITING...");
                Application.Quit();
                return;
            }
            KeyScreenCaster.PlayAnimationWithString("R");
            TrashCan.Recycle();
            _lastResetTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            onSetInputLock?.Invoke(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            onSetInputLock?.Invoke(true);
        } else if (Input.GetKeyDown(KeyCode.F1)) {
            ContextSensitiveHelper.Instance.SwitchHelpDisplay();
        }
    }
}
