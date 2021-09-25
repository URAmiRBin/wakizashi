using UnityEngine;
using UbiRock.Wakizashi.Toolkit;

namespace UbiRock.Wakizashi {
    public class MonoBehaviourSlicer : MonoBehaviour {
        public GameObject meshToSlice;
        public GameObject plane;

        public void Slice() {
            Vector3 position = plane.transform.position;
            Vector3 direction = plane.transform.up;

            Matrix4x4 mat = meshToSlice.transform.worldToLocalMatrix;
            Matrix4x4 transpose = mat.transpose;
            Matrix4x4 inv = transpose.inverse;

            Vector3 refUp = inv.MultiplyVector(direction).normalized;
            Vector3 refPt = meshToSlice.transform.InverseTransformPoint(position);


            Toolkit.Plane p = new Toolkit.Plane(refPt, refUp);

            Material material = meshToSlice.GetComponent<MeshRenderer>().material;
            
            SlicedHull result = Slicer.Slice(meshToSlice, p);
            GameObject topObject = result.CreateTopMesh(meshToSlice.transform, material);
            GameObject bottomObject = result.CreateBottomMesh(meshToSlice.transform, material);

            meshToSlice.SetActive(false);
        }
    }
}