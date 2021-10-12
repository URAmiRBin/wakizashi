using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextSensitiveHelper : MonoBehaviour {
    public enum Mode { Base = 4, Move = 0, ClickSlice = 1, PlaneSlice = 2, Creation = 3  }
    [SerializeField] Text helpText;
    [TextArea] [SerializeField] string baseHelp, moveHelp, clickSliceHelp, planeSliceHelp, creationHelp;

    string[] helps = new string[5];

    List<Mode> activeModes = new List<Mode>() {Mode.Base};

    public static ContextSensitiveHelper Instance;

    void Awake() {
        Instance = this;
        helps[0] = moveHelp;
        helps[1] = clickSliceHelp;
        helps[2] = planeSliceHelp;
        helps[3] = creationHelp;
        helps[4] = baseHelp;
    }

    public void AddHelp(Mode mode) {
        if (!activeModes.Contains(mode)) {
            activeModes.Add(mode);
            UpdateHelpText();
        }
    }

    public void RemoveHelp(Mode mode) {
        activeModes.Remove(mode);
        UpdateHelpText();
    }

    void UpdateHelpText() {
        helpText.text = "";
        activeModes.Sort((x, y) => (int)x < (int)y ? -1 : 1);
        foreach(Mode mode in activeModes) {
            helpText.text += helps[(int)mode];
        }
    }

    public void SwitchHelpDisplay() {
        foreach(Transform child in transform) child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
    }
}
