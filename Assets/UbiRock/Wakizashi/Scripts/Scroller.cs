using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour {
    [SerializeField] string[] items;
    Animator _animator;
    Text _nowText, _nextText;
    int _currentItemIndex;
    bool _isScrolling;
    WaitForEndOfFrame _frameBreather = new WaitForEndOfFrame();

    public int NextItemIndex => (_currentItemIndex + 1) % items.Length;

    void Awake() {
        _animator = GetComponent<Animator>();
        _nowText = transform.GetChild(0).GetComponent<Text>();
        _nextText = transform.GetChild(1).GetComponent<Text>();
    }

    void Start() {
        _nowText.text = items[_currentItemIndex];
        _nextText.text = items[(_currentItemIndex + 1) % items.Length];
    }

    public bool Scroll() {
        if (_isScrolling) return false;
        _animator.SetTrigger("scroll");
        return _isScrolling = true;
    }

    public void Reset() {
        if (_isScrolling) return;
        _nextText.text = items[0];
        _currentItemIndex = items.Length - 1;
        _animator.SetTrigger("scroll");
    }

    public void UpdateScrollText() => StartCoroutine(UpdateScrollTextCoroutine());

    IEnumerator UpdateScrollTextCoroutine() {
        yield return _frameBreather;
        _currentItemIndex = (_currentItemIndex + 1) % items.Length;
        _nowText.text = items[_currentItemIndex];
        _nextText.text = items[(_currentItemIndex + 1) % items.Length];
        _isScrolling = false;
    }
}
