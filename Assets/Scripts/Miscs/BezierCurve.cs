using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    public static Vector3 QuadraticBezierCurve(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint,float t)
    {
        return Vector3.Lerp(
            Vector3.Lerp(startPoint, controlPoint, t),
            Vector3.Lerp(controlPoint, endPoint, t),
            t);
    }

    public static Vector3 CubicBezierCurve(Vector3 startPoint, Vector3 controlPointStart, Vector3 controlPointEnd, Vector3 endPoint, float t)
    {
        Vector3 A = Vector3.Lerp(startPoint, controlPointStart, t);
        Vector3 B = Vector3.Lerp(controlPointStart, controlPointEnd, t);
        Vector3 C = Vector3.Lerp(controlPointEnd, endPoint, t);

        return QuadraticBezierCurve(A, B, C, t);
    }
}
