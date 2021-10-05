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

        public GameObject CreateTopMesh(Transform transform, Material material, Material sliceMaterial, bool physics) {
            return CreateObjectAt(_topMesh, transform, material, sliceMaterial, physics);
        }

        public GameObject CreateBottomMesh(Transform transform, Material material, Material sliceMaterial, bool physics) {
            return CreateObjectAt(_bottomMesh, transform, material, sliceMaterial, physics);
        }

        private GameObject CreateObjectAt(Mesh mesh, Transform transform, Material material, Material sliceMaterial, bool physics, string name = "generated mesh") {
            GameObject obj = new GameObject(name);
            obj.transform.localPosition = transform.localPosition;
            obj.transform.localRotation = transform.localRotation;
            obj.transform.localScale = transform.localScale;

            System.Action callback = null;
            if (physics) callback = () => obj.AddComponent<Rigidbody>();

            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            Tweener.Instance.MoveAlong(obj.transform, normal, .5f, .4f, EaseType.CircInOut, callback);
            MeshCollider collider = obj.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = true;
            obj.layer = 6;

            meshFilter.mesh = mesh;
            meshRenderer.sharedMaterials = new Material[] {material};

            return obj;
        }
    }
}