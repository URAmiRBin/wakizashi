using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour {
    [SerializeField] string[] items;
    Animator _animator;
    Text _nowText, _nextText;
    int _currentItemIndex;
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

    public void Scroll() => _animator.SetTrigger("scroll");

    public void UpdateScrollText() => StartCoroutine(UpdateScrollTextCoroutine());

    IEnumerator UpdateScrollTextCoroutine() {
        yield return _frameBreather;
        _currentItemIndex = (_currentItemIndex + 1) % items.Length;
        _nowText.text = items[_currentItemIndex];
        _nextText.text = items[(_currentItemIndex + 1) % items.Length];
    }
}
