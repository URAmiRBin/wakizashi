using System;
using System.Collections;
using UnityEngine;

namespace UbiRock.Utils {
    public class Tweener : MonoBehaviour {
        public static Tweener Instance;

        void Awake() => Instance = this;
        

        IEnumerator MoveAlongCoroutine(Transform transform, Vector3 direction, float duration = 1, float amount = 1, EaseType type = EaseType.Linear, Action callback = null) {
            direction = direction.normalized;
            Vector3 originalPosition = transform.position;
            float t = 0;
            while (t <= duration) {
                // FIXME: This is totally wrong
                transform.position = originalPosition + direction * Easing.Ease(Mathf.Lerp(0, amount, t / duration), type);
                t += Time.deltaTime;
                yield return null;
            }
            if (callback != null) callback();
        }

        IEnumerator ScalePopCoroutine(Transform transform, float startScalePercent, float duration, EaseType type) {
            Vector3 targetScale = transform.localScale;
            Vector3 startScale = targetScale * startScalePercent;
            float t = 0;
            while (t <= duration) {
                transform.localScale = Vector3.Lerp(startScale, targetScale, t / duration) * Easing.Ease(Mathf.Lerp(startScalePercent, 1, t / duration), type);
                t += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
        }

        public void MoveAlong(Transform transform, Vector3 direction, float duration = 1, float amount = 1, EaseType type = EaseType.Linear, Action callback = null) {
            StartCoroutine(MoveAlongCoroutine(transform, direction, duration, amount, type, callback));
        }

        public void ScalePop(Transform transform, float startScalePercent, float duration = 0.5f, EaseType type = EaseType.Linear) {
            StartCoroutine(ScalePopCoroutine(transform, startScalePercent, duration, type));
        }
    }
}
