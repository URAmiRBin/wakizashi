using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFitter : MonoBehaviour {
    MeshRenderer _renderer;
    MaterialPropertyBlock materialPropertyBlock;
    static readonly int shPropMainTexST = Shader.PropertyToID("_MainTex_ST");

    void Awake() => _renderer = GetComponent<MeshRenderer>();

    public MaterialPropertyBlock Mpb {
        get {
            if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();
            return materialPropertyBlock;
        }
    }

    public void FitTile() {
        Mpb.SetVector(shPropMainTexST, new Vector4(transform.localScale.x / 3f, transform.localScale.z / 3f, 0, 0));
        _renderer.SetPropertyBlock(Mpb, 0);
    }
}
