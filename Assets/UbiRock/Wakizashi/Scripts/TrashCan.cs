using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour {
    static Transform t;
    
    void Awake() => t = transform;
    public static void Recycle() {
        foreach(Transform child in t) Destroy(child.gameObject);
    }
}
