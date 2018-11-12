using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class responsible for drawing the road to the screen, and for storing the position of its left and right edges in line renderers.
/// </summary>
public class RoadRenderer : MonoBehaviour {

    public float horizon = 0f; // The y position of the horizon.

    // How far in either direction the car has to travel to die. (This should be replaced with a better system for collision detection eventually.)
    public float deathOffset;

    [SerializeField] public RoadLineCurver leftEdgeCurve;
    [SerializeField] public RoadLineCurver rightEdgeCurve;

    [HideInInspector] public Vector3 vanishingPointOffset = Vector3.zero;
    private float lowerOffset = 0f;
    [HideInInspector] public float roadWidth = 50f;
    [HideInInspector] public Vector3 curveControlPointOffset = Vector3.zero;

    // Used in collision detection for edges of the road. Should be replaced with a better system.
    float previousLowerOffset = 0f;


    private void Update() {

        // Set the lower offset based on the direction the car is currently turning.
        lowerOffset -= Services.car.turningValue;

        // Set the position of the vanishing point.
        Vector3 newVanishingPoint = new Vector3(vanishingPointOffset.x + lowerOffset * 0.1f, horizon, 0f);
        leftEdgeCurve.upperPoint = newVanishingPoint;
        rightEdgeCurve.upperPoint = newVanishingPoint;

        // Set road width
        leftEdgeCurve.lowerPoint = new Vector3((roadWidth * -0.5f) + lowerOffset, leftEdgeCurve.lowerPoint.y, 0f);
        rightEdgeCurve.lowerPoint = new Vector3((roadWidth * 0.5f) + lowerOffset, rightEdgeCurve.lowerPoint.y, 0f);

        // Set curve control point positions for road edges
        Vector3 _controlPointOffset = curveControlPointOffset;
        _controlPointOffset.x += lowerOffset * 0.15f;
        leftEdgeCurve.curveControlPoint = Vector3.Lerp(leftEdgeCurve.lowerPoint, leftEdgeCurve.upperPoint, 0.75f) + _controlPointOffset;
        rightEdgeCurve.curveControlPoint = Vector3.Lerp(rightEdgeCurve.lowerPoint, rightEdgeCurve.upperPoint, 0.75f) + _controlPointOffset;

        // See if I died
        if (leftEdgeCurve.lowerPoint.x > -deathOffset || rightEdgeCurve.lowerPoint.x < deathOffset) {
            lowerOffset = previousLowerOffset;
            Services.gameManager.Crash();
        }

        previousLowerOffset = lowerOffset;
    }

    /// <summary>
    /// Gets a position that is a certain percentage of the way across the road. 0 is all the way to the left, 1 is all the way to the right.
    /// </summary>
    public Vector3 GetRoadPosition(float roadPercentage, float distance) {
        Vector3 leftRoadPosition = MovePointDownLine(distance, leftEdgeCurve.m_LineRenderer);
        Vector3 rightRoadPosition = MovePointDownLine(distance, rightEdgeCurve.m_LineRenderer);
        return Vector3.Lerp(leftRoadPosition, rightRoadPosition, roadPercentage);
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
}
