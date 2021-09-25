using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class SlicedHull {
        private Mesh _topMesh, _bottomMesh;

        public SlicedHull(Mesh topMesh, Mesh bottomMesh) {
            _topMesh = topMesh;
            _bottomMesh = bottomMesh;
        }

        public GameObject CreateTopMesh(Transform refereceTransform) {
            return CreateTopMesh(refereceTransform.localPosition);
        }

        public GameObject CreateBottomMesh(Transform refereceTransform) {
            return CreateBottomMesh(refereceTransform.localPosition);
        }

        private GameObject CreateTopMesh(Vector3 position) {
            return CreateObjectAt(_topMesh, position);
        }

        private GameObject CreateBottomMesh(Vector3 position) {
            return CreateObjectAt(_bottomMesh, position);
        }

        private GameObject CreateObjectAt(Mesh mesh, Vector3 position, string name = "generated mesh") {
            GameObject obj = new GameObject(name);

            obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();

            meshFilter.mesh = mesh;

            return obj;
        }
    }
}