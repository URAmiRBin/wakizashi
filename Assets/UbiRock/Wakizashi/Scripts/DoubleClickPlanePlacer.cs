using UnityEngine;
using System.Collections.Generic;
using UbiRock.Wakizashi;

public class DoubleClickPlanePlacer : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] MonoBehaviourSlicer slicer;
    [SerializeField] LineUI line;
    private HashSet<GameObject> hits = new HashSet<GameObject>();
    Vector3 startWorldPosition, finishWorldPosition;
    Vector3 lastMousePosition;

    

    void Update()
    {
        lastMousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0)) {
            if (line.phase == LineDrawPhase.WaitingForFirstClick) {
                lastMousePosition = Input.mousePosition;
                line.SetStartPoint(lastMousePosition);
                lastMousePosition.z = Camera.main.nearClipPlane;
                startWorldPosition = Camera.main.ScreenToWorldPoint(lastMousePosition);
            } else {
                lastMousePosition = Input.mousePosition;
                line.ResetAndDeactivate();
                lastMousePosition.z = Camera.main.nearClipPlane;
                finishWorldPosition = Camera.main.ScreenToWorldPoint(lastMousePosition);
                FindAndSlice();
            }
        } else if (line.phase == LineDrawPhase.WaitingForSecondClick) {
            line.SetEndPoint(lastMousePosition);
        }
    }

    void FindAndSlice()
    {
        RaycastHit hit;
        for (float t = 0; t < 1; t += 0.1f) {
            Vector3 direction = Vector3.Lerp(startWorldPosition - transform.position, finishWorldPosition - transform.position, t);
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 100, _layerMask))
                hits.Add(hit.collider.gameObject);
        }

        slicer.SetSlicePlane(new UbiRock.Wakizashi.Toolkit.Plane(transform.position, startWorldPosition, finishWorldPosition));

        foreach(GameObject go in hits) {
            slicer.meshToSlice = go;
            slicer.Slice();
        }

        hits.Clear();
    }

}