using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class SlicedHull {
        private Mesh _topMesh, _bottomMesh;

        public static Vector3 normal;

        public SlicedHull(Mesh topMesh, Mesh bottomMesh) {
            _topMesh = topMesh;
            _bottomMesh = bottomMesh;
        }

        public GameObject CreateTopMesh(Transform transform, Material material, Material sliceMaterial) {
            return CreateObjectAt(_topMesh, transform, material, sliceMaterial);
        }

        public GameObject CreateBottomMesh(Transform transform, Material material, Material sliceMaterial) {
            return CreateObjectAt(_bottomMesh, transform, material, sliceMaterial);
        }

        private GameObject CreateObjectAt(Mesh mesh, Transform transform, Material material, Material sliceMaterial, string name = "generated mesh") {
            GameObject obj = new GameObject(name);
            obj.transform.localPosition = transform.localPosition;
            obj.transform.localRotation = transform.localRotation;
            obj.transform.localScale = transform.localScale;

            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            obj.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(5f, 10f), transform.position, 0f, -normal.y, ForceMode.VelocityChange);
            MeshCollider collider = obj.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.convex = true;
            

            meshFilter.mesh = mesh;
            meshRenderer.sharedMaterials = new Material[] {material};

            return obj;
        }
    }
}