using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScreenCaster : MonoBehaviour {
    Animator _animator;
    Text _keyText;
    Image _image;

    [SerializeField] Sprite _lockImage, _unlockImage;

    Color onColor = new Color(1, 1, 1, 1);
    Color warnColor = new Color(1, .25f, .25f, 1);
    Color offColor = new Color(1, 1, 1, 0);

    bool Locked => _image.sprite == _lockImage;

    void Awake() {
        _animator = GetComponent<Animator>();
        _keyText = GetComponentInChildren<Text>();
        _image = GetComponentsInChildren<Image>()[1];
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PlayAnimationWithString("ESC");
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            PlayAnimationWithString("Space");
        }
        else if (Input.GetKeyDown(KeyCode.L)) {
            PlayAnimationWithImage(_image.sprite == _lockImage ? _unlockImage : _lockImage, onColor);
        }
    }

    public void ResetCastTrigger() => _animator.SetBool("cast", false);

    void PlayAnimationWithString(string text) {
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

    void PlayAnimationWithImage(Sprite sprite, Color color) {
        if (_animator.GetBool("cast")) return;
        _keyText.color = offColor;
        _image.color = color;
        _image.sprite = sprite;
        if (_animator.GetBool("cast")) _animator.Play("idle", -1, 0f);
        _animator.SetBool("cast", true);
    }
}
