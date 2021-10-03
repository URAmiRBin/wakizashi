using UnityEngine;

public class LineUI : MonoBehaviour {
    GameObject _lineGameObject, _startBulletGameObject, _endBulletGameObject;
    RectTransform _lineTransform, _startBulletTransform, _endBulletTransform;

    void Awake() {
        _lineGameObject = transform.GetChild(0).gameObject;
        _startBulletGameObject = transform.GetChild(1).gameObject;
        _endBulletGameObject = transform.GetChild(2).gameObject;

        _lineTransform = _lineGameObject.GetComponent<RectTransform>();
        _startBulletTransform = _startBulletGameObject.GetComponent<RectTransform>();
        _endBulletTransform = _endBulletGameObject.GetComponent<RectTransform>();
    }

    public void SetStartPoint(Vector3 position) {
        _lineTransform.position = position;
        _lineGameObject.SetActive(true);
        _startBulletTransform.position = position;
        _startBulletGameObject.SetActive(true);
    }

    public void SetEndPoint(Vector3 position) {
        _lineTransform.sizeDelta = new Vector2(Vector2.Distance(_lineTransform.position, position), 2);
        _lineTransform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, position - _lineTransform.position));

        _endBulletTransform.position = position;
        _endBulletGameObject.SetActive(true);
    }

    public void ResetAndDeactivate() {
        _lineTransform.position = Vector3.zero;
        _lineTransform.sizeDelta = Vector2.zero;
        _lineGameObject.SetActive(false);
        _startBulletGameObject.SetActive(false);
        _endBulletGameObject.SetActive(false);
    }
}
