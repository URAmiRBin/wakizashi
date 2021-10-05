using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour {
    [SerializeField] float delayInSeconds;
    
    void Awake() => StartCoroutine(WaitAndFade(delayInSeconds));

    IEnumerator WaitAndFade(float delay) {
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetTrigger("fade");
    }
}
