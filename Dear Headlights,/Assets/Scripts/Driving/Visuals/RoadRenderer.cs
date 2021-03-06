﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class responsible for drawing the road to the screen, and for storing the position of its left and right edges in line renderers.
/// </summary>
public class RoadRenderer : MonoBehaviour {

    public float horizon = 0f; // The y position of the horizon line.

    private RoadLineCurver[] lineCurves;

    [HideInInspector] public Vector3 vanishingPointOffset = Vector3.zero;
    [HideInInspector] public float lowerOffset = 0f;
    [SerializeField] public float roadWidth = 50f;
    [HideInInspector] public Vector3 curveControlPointOffset = Vector3.zero;

    // Used in collision detection for edges of the road. Should be replaced with a better system.
    [HideInInspector] public float previousLowerOffset = 0f;
    
    [HideInInspector] public RoadLineCurver leftEdgeCurve { get { return lineCurves[0]; } }
    [HideInInspector] public RoadLineCurver rightEdgeCurve { get { return lineCurves[lineCurves.Length - 1]; } }

    private void Awake() {
        lineCurves = GetComponentsInChildren<RoadLineCurver>();
    }

    private void Update() {

        // Set the lower offset based on the direction the car is currently turning.
        lowerOffset = Den.Math.Map(Services.playerCar.roadPosition, -1f, 1f, roadWidth * 0.5f, roadWidth * -0.5f);

        // Set the position of the vanishing point.
        Vector3 newVanishingPoint = new Vector3(vanishingPointOffset.x + lowerOffset * 0.1f, horizon, 0f);

        foreach (RoadLineCurver lineCurve in lineCurves) {
            lineCurve.upperPoint = newVanishingPoint;

            // Set road width
            lineCurve.lowerPoint = new Vector3((roadWidth * lineCurve.roadPosition) + lowerOffset, lineCurve.lowerPoint.y, 0f);

            // Set curve control point positions for road edges
            lineCurve.curveControlPoint = Vector3.Lerp(lineCurve.lowerPoint, lineCurve.upperPoint, 0.75f) + curveControlPointOffset;
        }

        previousLowerOffset = lowerOffset;
    }

    /// <summary>
    /// Gets a position that is a certain distance across the road. -1 is the left edge of the road, 1 is the right edge, and 0 is the very center.
    /// </summary>
    public Vector3 GetRoadPosition(float roadPositionValue, float yPosition) {
        Vector3 leftRoadPosition = GetYPositionOnLine(yPosition, leftEdgeCurve.m_LineRenderer);
        Vector3 rightRoadPosition = GetYPositionOnLine(yPosition, rightEdgeCurve.m_LineRenderer);

        Vector3 position = Vector3.Lerp(leftRoadPosition, rightRoadPosition, 0.5f);

        position.x += roadPositionValue * (Vector3.Distance(leftRoadPosition, rightRoadPosition) * 0.5f);

        return position;
    }

    /// <summary>
    /// Gets a position on a line renderer a that is a certain percentage of the way from it's first position to it's last position.
    /// </summary>
    public Vector3 MovePointDownLine(float distancePercentage, LineRenderer lineRenderer) {
        float totalLineLength = 0f;
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            if (i + 1 >= lineRenderer.positionCount) { break; }
            totalLineLength += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
        }

        distancePercentage = Mathf.Clamp01(distancePercentage);

        float realDistance = Den.Math.Map(distancePercentage, 0f, 1f, 0f, totalLineLength);

        float distanceMoved = 0f;
        for (int i = 0; i < lineRenderer.positionCount - 1; i++) {

            // Length of the next segment
            float nextSegmentLength = Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));

            // See if this segment includes the point we're looking for
            if (distanceMoved + nextSegmentLength < realDistance) {
                distanceMoved += nextSegmentLength;
            }

            // If this the point IS on the next segment
            else {
                float distanceLeftToMove = realDistance - distanceMoved;
                float segmentPercentage = Den.Math.Map(distanceLeftToMove, 0f, nextSegmentLength, 0f, 1f);
                return Vector3.Lerp(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1), segmentPercentage);
            }
        }

        Debug.LogError("hey this thing fucking doesnt work dude");
        return Vector3.zero;
    }

    public Vector3 GetYPositionOnLine(float yPosition, LineRenderer lineRenderer) {

        bool nextSegmenetDidntInclude = false;
        float nextPositionY = 0f;

        for (int i = 0; i < lineRenderer.positionCount - 1; i++) {

            if (i == lineRenderer.positionCount - 2 && lineRenderer.GetPosition(i + 1).y > yPosition) {
                yPosition = lineRenderer.GetPosition(i + 1).y;
            }

            // See if this segment includes the point we're looking for
            if (lineRenderer.GetPosition(i + 1).y > yPosition) {
                nextSegmenetDidntInclude = true;
                nextPositionY = lineRenderer.GetPosition(i + 1).y;
                continue;
            }

            // If this the point IS on the next segment
            else {
                bool intersectionFound = false;
                Vector2 intersectPoint = GetIntersectionPointCoordinates(
                    Services.roadRenderer.transform.TransformPoint(new Vector2(-100f, yPosition)),
                    Services.roadRenderer.transform.TransformPoint(new Vector2(100f, yPosition)),
                    Services.roadRenderer.transform.TransformPoint(lineRenderer.GetPosition(i)),
                    Services.roadRenderer.transform.TransformPoint(lineRenderer.GetPosition(i + 1)),
                    out intersectionFound
                    );
                if (intersectionFound) {
                    return intersectPoint;
                }
            }
        }

        Debug.LogError("next segment didn't include: " + nextSegmenetDidntInclude);
        Debug.LogError("next position y: " + nextPositionY);
        Debug.LogError("my y: " + yPosition);
        //Debug.LogError("hey this thing fucking doesnt work dude");
        return new Vector3(10000f, 10000f, 0f);
    }

    Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found) {
        float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

        if (tmp == 0) {
            // No solution!
            found = false;
            return Vector2.zero;
        }

        float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

        found = true;

        return new Vector2(
            B1.x + (B2.x - B1.x) * mu,
            B1.y + (B2.y - B1.y) * mu
        );
    }
}
