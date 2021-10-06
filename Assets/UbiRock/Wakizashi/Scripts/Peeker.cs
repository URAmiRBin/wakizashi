using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peeker : MonoBehaviour {
    Animator _animator;

    void Awake() => _animator = GetComponent<Animator>();

    void Start() => StartCoroutine(WaitAndPeek());

    IEnumerator WaitAndPeek() {
        yield return new WaitForSeconds(Random.Range(25, 60));
        _animator.SetBool("peek", true);
        yield return new WaitForSeconds(Random.Range(2, 4));
        _animator.SetBool("peek", false);
        yield return WaitAndPeek();
    }
}
