using UnityEngine;
using UbiRock.Wakizashi.Toolkit;

namespace UbiRock.Wakizashi {
    public class MonoBehaviourSlicer : MonoBehaviour {
        public GameObject meshToSlice;
        public void Slice() {
            Toolkit.Plane p = new Toolkit.Plane(Vector3.zero, Vector3.up);
            SlicedHull result = Slicer.Slice(meshToSlice, p);
            GameObject topObject = result.CreateTopMesh(transform);
            GameObject bottomObject = result.CreateBottomMesh(transform);

            Instantiate(topObject, transform.position, transform.rotation);
            Instantiate(bottomObject, transform.position, transform.rotation);
        }
    }
}