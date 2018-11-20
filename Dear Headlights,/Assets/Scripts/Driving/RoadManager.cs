using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for controlling the layout of the road, for example choosing when the road should curve and in which direction.
/// </summary>
public class RoadManager : MonoBehaviour {

    // How long a segment of road lasts.
    [SerializeField] float baseTimePerSegment = 5f;

    RoadCurveSegment[] roadSegments;
    float segmentTimer = 0f;
    bool segmentFinished = true;

    private void Awake() {
        roadSegments = GetComponentsInChildren<RoadCurveSegment>();
    }

    private void Update() {
        if (segmentFinished) {
            BeginNewSegment();
        }
    }

    /// <summary>
    /// Begins the process of choosing and applying a new road segment. The possible segments are stored as monobehaviors on game objects parented to this script's game object.
    /// </summary>
    void BeginNewSegment() {
        segmentFinished = false;

        // Choose a new segment at random.
        RoadCurveSegment nextSegment = roadSegments[Random.Range(0, roadSegments.Length)];
        Debug.Log("Begining segment:" + nextSegment.name);

        // Begin the process of applying the chosen segment to the road.
        StartCoroutine(ApplyCurveSequence(nextSegment));
    }

    IEnumerator ApplyCurveSequence(RoadCurveSegment segment) {
        // Store the variables to lerp from.
        Vector3 startingVanishingPointOffset = Services.roadRenderer.vanishingPointOffset;
        float startingSteeringInfluence = Services.car.steeringInfluenceFromCurve;
        Vector3 startingControlPointOffset = Services.roadRenderer.curveControlPointOffset;

        // Lerp the road renderer's values to those of the chosen segment, and wait until the lerp finishes to continue.
        float lerpValue = 0f;
        float lerpDuration = 3.5f;
        yield return new WaitUntil(() => {
            lerpValue += Services.gameManager.drivingDeltaTime * Services.car.currentSpeed;
            if (lerpValue < lerpDuration) {
                Services.roadRenderer.vanishingPointOffset.x = Mathf.Lerp(startingVanishingPointOffset.x, segment.curvePower, Den.Math.Map(lerpValue, 0f, lerpDuration, 0f, 1f));
                Services.car.steeringInfluenceFromCurve = Mathf.Lerp(startingSteeringInfluence, segment.CarInfluence, Den.Math.Map(lerpValue, 0f, lerpDuration, 0f, 1f));
                Services.roadRenderer.curveControlPointOffset = Vector3.Lerp(startingControlPointOffset, segment.ControlPointOffset, Den.Math.Map(lerpValue, 0f, lerpDuration, 0f, 1f));
                return false;
            }

            else {
                Services.roadRenderer.vanishingPointOffset.x = segment.curvePower;
                return true;
            }
        });

        // Wait for the now-current segment to finish.
        float timer = 0f;
        yield return new WaitUntil(() => {
            timer += Services.gameManager.drivingDeltaTime * Services.car.currentSpeed;
            if (timer < baseTimePerSegment) {
                return false;
            }

            else {
                return true;
            }
        });

        segmentFinished = true;

        yield return null;
    }
}
