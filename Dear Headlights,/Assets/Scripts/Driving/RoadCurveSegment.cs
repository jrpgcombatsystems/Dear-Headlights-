using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information for a type of curve in the road.
/// </summary>
public class RoadCurveSegment : MonoBehaviour {

    public float curvePower = 0f;
    public float CarInfluence { get { return curvePower * -1.1f; } }
    public Vector3 ControlPointOffset {
        get {
            return new Vector3(curvePower * -0.9f, 0f, 0f);
        }
    }
}
