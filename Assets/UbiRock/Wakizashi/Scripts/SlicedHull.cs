using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class SlicedHull {
        private Mesh _topMesh, _bottomMesh;

        public SlicedHull(Mesh topMesh, Mesh bottomMesh) {
            _topMesh = topMesh;
            _bottomMesh = bottomMesh;
        }

        public GameObject CreateTopMesh(Transform position) {
            return CreateObjectAt(_topMesh, position);
        }

        public GameObject CreateBottomMesh(Transform position) {
            return CreateObjectAt(_bottomMesh, position);
        }

        private GameObject CreateObjectAt(Mesh mesh, Transform transform, string name = "generated mesh") {
            GameObject obj = new GameObject(name);
            obj.transform.localPosition = transform.localPosition;
            obj.transform.localRotation = transform.localRotation;

            obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();

            meshFilter.mesh = mesh;

            return obj;
        }
    }
}