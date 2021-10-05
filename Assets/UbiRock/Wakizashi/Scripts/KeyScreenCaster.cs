using UnityEngine;
using UnityEngine.UI;

public class KeyScreenCaster : MonoBehaviour {
    static Animator _animator;
    static Text _keyText;
    static Image _image;

    static Sprite _lockImage, _unlockImage;

    static Color onColor = new Color(1, 1, 1, 1);
    static Color warnColor = new Color(1, .25f, .25f, 1);
    static Color offColor = new Color(1, 1, 1, 0);

    static bool Locked => _image.sprite == _lockImage;

    void Awake() {
        InputManager.onSetInputLock += PlayAnimationLock;
        _animator = GetComponent<Animator>();
        _keyText = GetComponentInChildren<Text>();
        _image = GetComponentsInChildren<Image>()[1];
    }

    void Start() {
        _lockImage = Resources.Load<Sprite>("lock-locked-512");
        _unlockImage = Resources.Load<Sprite>("lock-unlocked-512");
    }

    public void ResetCastTrigger() => _animator.SetBool("cast", false);

    public static void PlayAnimationWithString(string text) {
        if (Locked) {
            PlayAnimationWithImage(_lockImage, warnColor);
            return;
        }
        if (_keyText.text == text && _keyText.color == onColor && _animator.GetBool("cast")) return;
        _image.color = offColor;
        _keyText.color = onColor;
        _keyText.text = text;
        if (_animator.GetBool("cast")) _animator.Play("idle", -1, 0f);
        _animator.SetBool("cast", true);
    }

    public static void PlayAnimationWithImage(Sprite sprite, Color color) {
        _keyText.color = offColor;
        _image.color = color;
        _image.sprite = sprite;
        if (_animator.GetBool("cast")) _animator.Play("idle", -1, 0f);
        _animator.SetBool("cast", true);
    }

    void PlayAnimationLock(bool isLocked) {
        if (isLocked) PlayAnimationWithImage(_lockImage, onColor);
        else PlayAnimationWithImage(_unlockImage, onColor);
    }
}
