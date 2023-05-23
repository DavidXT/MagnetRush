using System;
using System.Collections;
using UnityEngine;
public class BezierCurves {
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) {
        return a + (b - a) * t;
    }

    public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 p0 = Lerp(a, b, t);
        Vector3 p1 = Lerp(b, c, t);
        return Lerp(p0, p1, t);
    }

    public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 p0 = QuadraticCurve(a, b, c, t);
        Vector3 p1 = QuadraticCurve(b, c, d, t);
        return Lerp(p0, p1, t);
    }

    public static IEnumerator JumpIn(Transform that, Transform target, Vector3 targetOffset, float height, Vector3 halfPointOffset, float duration, bool setAsParent = false, Action action = null) {
        that.SetParent(null);

        Vector3 targetPosition = target.position + targetOffset;
        Vector3 initialPosition = that.position;

        Vector3 halfPoint = new Vector3((targetPosition.x + initialPosition.x) / 2, initialPosition.y + height, (targetPosition.z + initialPosition.z) / 2) + halfPointOffset;

        float _t = 0;
        float _targetT = duration;

        while (_t < _targetT && that) {
            float _ratioToEnd = _t / _targetT;

            if (target) {
                targetPosition = target.position + targetOffset;
            }

            that.position = QuadraticCurve(initialPosition, halfPoint, targetPosition, _ratioToEnd);

            _t += Time.deltaTime;
            yield return null;
        }

        if (target && that) {
            that.position = target.position + targetOffset;
            if (setAsParent) {
                that.SetParent(target);
            }
        }

        action?.Invoke();
    }
}