using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour {

    [SerializeField] float baseTimePerSegment = 5f;

    RoadSegment[] roadSegments;
    float segmentTimer = 0f;
    bool segmentFinished = true;

    private void Awake() {
        roadSegments = GetComponentsInChildren<RoadSegment>();
    }

    private void Update() {
        if (segmentFinished) {
            BeginNewSegment();
        }
    }

    void BeginNewSegment() {
        segmentFinished = false;
        RoadSegment nextSegment = roadSegments[Random.Range(0, roadSegments.Length)];
        Debug.Log("Begining segment:" + nextSegment.name);
        StartCoroutine(CurveSequence(nextSegment));
    }

    IEnumerator CurveSequence(RoadSegment segment) {

        // Lerp to segment max
        float startingUpperOffset = Services.roadRenderer.upperOffset;
        float startingCarInfluence = Services.roadRenderer.currentCarInfluence;
        float lerpValue = 0f;
        float lerpDuration = 3.5f;
        yield return new WaitUntil(() => {
            lerpValue += Time.deltaTime * Services.gameManager.currentSpeed;
            if (lerpValue < lerpDuration) {
                Services.roadRenderer.upperOffset = Mathf.Lerp(startingUpperOffset, segment.curveOffset, MyMath.Map(lerpValue, 0f, lerpDuration, 0f, 1f));
                Services.roadRenderer.currentCarInfluence = Mathf.Lerp(startingCarInfluence, segment.carInfluence, MyMath.Map(lerpValue, 0f, lerpDuration, 0f, 1f));
                return false;
            }

            else {
                Services.roadRenderer.upperOffset = segment.curveOffset;
                return true;
            }
        });

        // Change influence on car also... once there is a car

        // Wait for segment to finish
        float timer = 0f;
        yield return new WaitUntil(() => {
            timer += Time.deltaTime * Services.gameManager.currentSpeed;
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

    //RoadSegment GetRandomSegment() {
    //    int numberOfSegments = System.Enum.GetValues(typeof(RoadSegment)).Length;
    //    int nextSegmentIndex = Random.Range(0, numberOfSegments);
    //    return (RoadSegment) nextSegmentIndex;
    //}
}
