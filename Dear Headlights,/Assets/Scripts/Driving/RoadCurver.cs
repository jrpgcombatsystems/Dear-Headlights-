using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RoadCurver : MonoBehaviour {

    public Transform startPoint;
    public Transform endPoint;
    public Transform controlPoint;

    public LineRenderer m_LineRenderer { get { return GetComponent<LineRenderer>(); } }

    private void Update() {
        m_LineRenderer.SetPositions(BezierCurve.ThreePointCurve(startPoint.position, endPoint.position, controlPoint.position, m_LineRenderer.positionCount));
    }
}
