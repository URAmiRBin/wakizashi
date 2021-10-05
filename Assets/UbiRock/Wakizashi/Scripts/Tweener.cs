using System;
using System.Collections;
using UnityEngine;

namespace UbiRock.Utils {
    public class Tweener : MonoBehaviour {
        public static Tweener Instance;

        void Awake() {
            if (Instance != null) Destroy(this);
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        

        public static IEnumerator MoveAlongCoroutine(Transform transform, Vector3 direction, float duration = 1, float amount = 1, EaseType type = EaseType.Linear, Action callback = null) {
            direction = direction.normalized;
            Vector3 originalPosition = transform.position;
            float t = 0;
            while (t <= duration) {
                transform.position = originalPosition + direction * (float)Easing.Ease(Mathf.Lerp(0, amount, t / duration), type);
                t += Time.deltaTime;
                yield return null;
            }
            if (callback != null) callback();
        }

        public void MoveAlong(Transform transform, Vector3 direction, float duration = 1, float amount = 1, EaseType type = EaseType.Linear, Action callback = null) {
            StartCoroutine(MoveAlongCoroutine(transform, direction, duration, amount, type, callback));
        }
    }
}
