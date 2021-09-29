using UnityEngine;
using UbiRock.Wakizashi.Toolkit;

namespace UbiRock.Wakizashi {
    public class MonoBehaviourSlicer : MonoBehaviour {
        public GameObject meshToSlice;
        public GameObject plane;

        public Material sliceMaterial;

        public void Slice() {
            Matrix4x4 mat = meshToSlice.transform.worldToLocalMatrix;
            Matrix4x4 transpose = mat.transpose;
            Matrix4x4 inv = transpose.inverse;

            Vector3 refUp = inv.MultiplyVector(plane.transform.up).normalized;
            Vector3 refPt = meshToSlice.transform.InverseTransformPoint(plane.transform.position);


            Toolkit.Plane p = new Toolkit.Plane(refPt, refUp);

            Material material = meshToSlice.GetComponent<MeshRenderer>().material;
            
            SlicedHull result = Slicer.Slice(meshToSlice, p);
            SlicedHull.normal = p.Normal;
            GameObject topObject = result.CreateTopMesh(meshToSlice.transform, material, sliceMaterial);
            SlicedHull.normal = -p.Normal;
            GameObject bottomObject = result.CreateBottomMesh(meshToSlice.transform, material, sliceMaterial);

            meshToSlice.SetActive(false);
        }
    }
}