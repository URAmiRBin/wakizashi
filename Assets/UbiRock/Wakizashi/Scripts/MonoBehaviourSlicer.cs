using UnityEngine;
using UbiRock.Wakizashi.Toolkit;

namespace UbiRock.Wakizashi {
    public class MonoBehaviourSlicer : MonoBehaviour {
        public GameObject meshToSlice;
        public void Slice() {
            Vector3 position = Vector3.zero;
            Vector3 direction = Vector3.up;

            Matrix4x4 mat = meshToSlice.transform.worldToLocalMatrix;
            Matrix4x4 transpose = mat.transpose;
            Matrix4x4 inv = transpose.inverse;

            Vector3 refUp = inv.MultiplyVector(direction).normalized;
            Vector3 refPt = meshToSlice.transform.InverseTransformPoint(position);


            Toolkit.Plane p = new Toolkit.Plane(refPt, refUp);
            
            SlicedHull result = Slicer.Slice(meshToSlice, p);
            GameObject topObject = result.CreateTopMesh(meshToSlice.transform);
            GameObject bottomObject = result.CreateBottomMesh(meshToSlice.transform);
        }
    }
}