using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve {

	public static Vector3[] ThreePointCurve(Vector3 beginningPoint, Vector3 endPoint, Vector3 controlPoint, int pointsToReturn) {
        float time = 0f;
        float timeStep = 1 / (float) pointsToReturn;

        Vector3[] returnPoints = new Vector3[pointsToReturn];

        for (int i = 0; i < pointsToReturn; i++) {
            Vector3 point1 = Vector3.Lerp(beginningPoint, controlPoint, time);
            Vector3 point2 = Vector3.Lerp(controlPoint, endPoint, time);
            returnPoints[i] = Vector3.Lerp(point1, point2, time);
            time += timeStep;
        }

        return returnPoints;
    }
}
