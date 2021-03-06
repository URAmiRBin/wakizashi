using UnityEngine;
using UbiRock.Wakizashi.Toolkit;

namespace UbiRock.Wakizashi {
    public class MonoBehaviourSlicer : MonoBehaviour {
        [SerializeField] Transform garbage;
        public Sliceable meshToSlice;
        Toolkit.Plane _plane;
        public Material sliceMaterial;

        public void SetSlicePlane(Toolkit.Plane plane) => _plane = plane;
        public void SetSlicePlane(Transform planeTransform) {
            _plane = new Toolkit.Plane(planeTransform.position, planeTransform.up);
        }

        public void SetMeshToSlice(Sliceable m) => meshToSlice = m;

        public void Slice() {
            Matrix4x4 mat = meshToSlice.transform.worldToLocalMatrix;
            Matrix4x4 transpose = mat.transpose;
            Matrix4x4 inv = transpose.inverse;

            Vector3 refUp = inv.MultiplyVector(_plane.Normal).normalized;
            Vector3 refPt = meshToSlice.transform.InverseTransformPoint(_plane.Position);


            Toolkit.Plane p = new Toolkit.Plane(refPt, refUp);

            Material material = meshToSlice.GetComponent<MeshRenderer>().material;
            
            SlicedHull result = Slicer.Slice(meshToSlice, p);
            SlicedHull.normal = _plane.Normal;
            GameObject topObject = result.CreateTopMesh(meshToSlice, material, sliceMaterial);
            SlicedHull.normal = -_plane.Normal;
            GameObject bottomObject = result.CreateBottomMesh(meshToSlice, material, sliceMaterial);

            meshToSlice.gameObject.SetActive(false);

            bottomObject.transform.parent = garbage;
            topObject.transform.parent = garbage;
            meshToSlice.transform.parent = garbage;
        }
    }
}