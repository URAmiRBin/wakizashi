using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersionIndicator : MonoBehaviour {
    void Start() => GetComponent<Text>().text = "v" + Application.version;
}
