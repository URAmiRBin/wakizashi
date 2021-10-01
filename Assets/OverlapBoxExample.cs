//Attach this script to your GameObject. This GameObject doesn’t need to have a Collider component
//Set the Layer Mask field in the Inspector to the layer you would like to see collisions in (set to Everything if you are unsure).
//Create a second Gameobject for testing collisions. Make sure your GameObject has a Collider component (if it doesn’t, click on the Add Component button in the GameObject’s Inspector, and go to Physics>Box Collider).
//Place it so it is overlapping your other GameObject.
//Press Play to see the console output the name of your second GameObject

//This script uses the OverlapBox that creates an invisible Box Collider that detects multiple collisions with other colliders. The OverlapBox in this case is the same size and position as the GameObject you attach it to (acting as a replacement for the BoxCollider component).

using UnityEngine;
using System.Collections.Generic;
using UbiRock.Wakizashi;

public class OverlapBoxExample : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    public GameObject indicator;
    public MonoBehaviourSlicer slicer;
    public GameObject plani;
    

    private GameObject[] _indicatorPool = new GameObject[2];

    private HashSet<GameObject> hits = new HashSet<GameObject>();

    void Start()
    {
        _indicatorPool[0] = Instantiate(indicator);
        _indicatorPool[1] = Instantiate(indicator);
        _indicatorPool[0].SetActive(false);
        _indicatorPool[1].SetActive(false);
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
    }

    bool clickOne, clickTwo;
    Vector3 s, f;

    Vector3 firstMouse, secondMouse;

    float width, r;

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !clickOne && !clickTwo) {
            clickOne = true;
            firstMouse = Input.mousePosition;
            firstMouse.z = Camera.main.nearClipPlane + .1f;
            s = Camera.main.ScreenToWorldPoint(firstMouse);
            _indicatorPool[0].transform.position = s;
            _indicatorPool[0].SetActive(true);
        } else if (Input.GetMouseButtonDown(0) && clickOne && !clickTwo) {
            clickTwo = true;
            secondMouse = Input.mousePosition;
            secondMouse.z = Camera.main.nearClipPlane + .1f;
            f = Camera.main.ScreenToWorldPoint(secondMouse);
            _indicatorPool[1].transform.position = f;
            _indicatorPool[1].SetActive(true);
            width = Vector3.Distance(s, f);
            r = Vector3.SignedAngle(Vector3.right, s - f, transform.forward);
            MyCollisions();
        }
    }

    void MyCollisions()
    {
        RaycastHit hit;
        for (float t = 0; t < 1; t += 0.1f) {
            Vector3 dir = Vector3.Lerp(s - transform.position, f - transform.position, t);
            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100, m_LayerMask)) {
                hits.Add(hit.collider.gameObject);
            }
        }


        foreach(GameObject go in hits) {
            Vector3 pos1 = firstMouse;
            pos1.z = Vector3.Distance(transform.position, go.transform.position);
            pos1 = Camera.main.ScreenToWorldPoint(pos1);
            Vector3 pos2 = firstMouse;
            pos2.z = Vector3.Distance(transform.position, go.transform.position);
            pos2 = Camera.main.ScreenToWorldPoint(pos2);

            plani.transform.position = new Vector3(pos1.x, pos2.y, pos2.z);
            float f = transform.rotation.eulerAngles.x;
            if (r < -90) plani.transform.rotation = Quaternion.Euler(f, 0f, 180f + r);
            else if (r > 90) plani.transform.rotation = Quaternion.Euler(f, 0f, r - 180f);
            else plani.transform.rotation = Quaternion.Euler(f, 0f, r);

            Vector3 normal = Vector3.Cross(pos1 - pos2, transform.up);
            Debug.Log(normal);
            UbiRock.Wakizashi.Toolkit.Plane p = new UbiRock.Wakizashi.Toolkit.Plane(s, Vector3.zero);
            slicer.meshToSlice = go;
            slicer.plane = plani;
            slicer.Slice();
        }

        clickOne = clickTwo = false;
        hits.Clear();
        _indicatorPool[0].SetActive(false);
        _indicatorPool[1].SetActive(false);
    }

}