using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    [HideInInspector] public float upperOffset = 0f;
    public float horizon = 0f;
    [HideInInspector] public float width = 50f;
    [HideInInspector] public float lowerOffset = 0f;
    [HideInInspector] public float controlPointYOffset = 0f;
    [HideInInspector] public float currentCarInfluence = 0f;
    public float deathOffset;

    float previousLowerOffset = 0f;

    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float accelerationSpeed = 1f;

    [SerializeField] public RoadCurver leftRoad;
    [SerializeField] public RoadCurver rightRoad;

    private void Update() {

        // Get horizontal input.
        lowerOffset += Input.GetAxis("Horizontal") * -turnSpeed * Services.gameManager.currentSpeed * Time.deltaTime;

        // Speed up
        if (Input.GetAxisRaw("Vertical") > 0) {
            Services.gameManager.currentSpeed += accelerationSpeed * Time.deltaTime;
        }

        // Set vanishing point
        Vector3 newVanishingPoint = new Vector3(upperOffset, horizon, 0f);
        leftRoad.startPoint.localPosition = newVanishingPoint;
        rightRoad.startPoint.localPosition = newVanishingPoint;

        lowerOffset += currentCarInfluence * Services.gameManager.currentSpeed * Time.deltaTime;

        // Set road width
        leftRoad.endPoint.position = new Vector3((width * -0.5f) + lowerOffset, leftRoad.endPoint.position.y, 0f);
        rightRoad.endPoint.position = new Vector3((width * 0.5f) + lowerOffset, rightRoad.endPoint.position.y, 0f);

        // Set road control point positions
        Vector3 newPosition = leftRoad.controlPoint.position;
        newPosition.y += controlPointYOffset;
        leftRoad.controlPoint.position = newPosition;
        newPosition = rightRoad.controlPoint.position;
        newPosition.y += controlPointYOffset;
        leftRoad.controlPoint.position = newPosition;

        // See if I died
        if (leftRoad.endPoint.position.x > -deathOffset || rightRoad.endPoint.position.x < deathOffset) {
            lowerOffset = previousLowerOffset;
            Services.gameManager.Crash();
        }

        previousLowerOffset = lowerOffset;
    }

    public Vector3 GetRoadPosition(float roadPercentage, float distance) {
        Vector3 leftRoadPosition = MovePointDownLine(distance, leftRoad.m_LineRenderer);
        Vector3 rightRoadPosition = MovePointDownLine(distance, rightRoad.m_LineRenderer);
        return Vector3.Lerp(leftRoadPosition, rightRoadPosition, roadPercentage);
    }

    public Vector3 MovePointDownLine(float distancePercentage, LineRenderer lineRenderer) {
        float totalLineLength = 0f;
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            if (i + 1 >= lineRenderer.positionCount) { break; }
            totalLineLength += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
        }

        distancePercentage = Mathf.Clamp01(distancePercentage);

        float realDistance = MyMath.Map(distancePercentage, 0f, 1f, 0f, totalLineLength);

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
                float segmentPercentage = MyMath.Map(distanceLeftToMove, 0f, nextSegmentLength, 0f, 1f);
                return Vector3.Lerp(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1), segmentPercentage);
            }
        }

        Debug.LogError("hey this thing fucking doesnt work dude");
        return Vector3.zero;
    }
}
