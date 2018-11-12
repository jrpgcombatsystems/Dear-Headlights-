using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for drawing the curve of a given edge of the road.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class RoadLineCurver : MonoBehaviour {

    [SerializeField] private Transform upperPointTransform;
    [SerializeField] private Transform lowerPointTransform;
    [SerializeField] private Transform controlPointTransform;

    public Vector3 upperPoint {
        get { return upperPointTransform.position; }
        set { upperPointTransform.position = value; }
    }
    public Vector3 lowerPoint {
        get { return lowerPointTransform.position; }
        set { lowerPointTransform.position = value; }
    }
    public Vector3 curveControlPoint {
        get { return controlPointTransform.position;  }
        set { controlPointTransform.position = value; }
    }

    public LineRenderer m_LineRenderer { get { return GetComponent<LineRenderer>(); } }

    private void Update() {
        m_LineRenderer.SetPositions(Den.Geometry.ThreePointBezierCurve(upperPoint, lowerPoint, curveControlPoint, m_LineRenderer.positionCount));
    }
}
