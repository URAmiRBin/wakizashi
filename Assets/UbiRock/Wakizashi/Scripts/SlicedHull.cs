using UnityEngine;
using UbiRock.Utils;

namespace UbiRock.Wakizashi.Toolkit {
    public class SlicedHull {
        private Mesh _topMesh, _bottomMesh;

        public static Vector3 normal;

        public SlicedHull(Mesh topMesh, Mesh bottomMesh) {
            _topMesh = topMesh;
            _bottomMesh = bottomMesh;
        }

        public GameObject CreateTopMesh(Sliceable sliceable, Material material, Material sliceMaterial) {
            return CreateObjectAt(_topMesh, sliceable, material, sliceMaterial);
        }

        public GameObject CreateBottomMesh(Sliceable sliceable, Material material, Material sliceMaterial) {
            return CreateObjectAt(_bottomMesh, sliceable, material, sliceMaterial);
        }

        private GameObject CreateObjectAt(Mesh mesh, Sliceable sliceable, Material material, Material sliceMaterial, string name = "generated mesh") {
            GameObject obj = new GameObject(name);
            obj.transform.localPosition = sliceable.transform.localPosition;
            obj.transform.localRotation = sliceable.transform.localRotation;
            obj.transform.localScale = sliceable.transform.localScale;

            System.Action callback = null;
            if (sliceable.Physics) callback = () => obj.AddComponent<Rigidbody>();

            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            Tweener.Instance.MoveAlong(obj.transform, normal, .5f, .4f, EaseType.CircInOut, callback);
            MeshCollider collider = obj.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = true;
            obj.layer = 6;

            meshFilter.mesh = mesh;
            meshRenderer.sharedMaterials = new Material[] {material};
            Sliceable newSliceable = obj.AddComponent<Sliceable>();
            newSliceable.SetOptions(sliceable.Physics, sliceable.Fill);

            return obj;
        }
    }
}