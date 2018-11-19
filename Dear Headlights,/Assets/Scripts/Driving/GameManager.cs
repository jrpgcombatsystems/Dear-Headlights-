using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [HideInInspector] public float drivingTime;
    public float drivingTimeScale = 1;
    [HideInInspector] public float drivingDeltaTime = 0f;

    private float lastFrameTime = 0f;

    private void Awake() {
        // Locate and store references to various important classes.
        Services.gameManager = this;
        Services.roadManager = FindObjectOfType<RoadManager>();
        Services.roadRenderer = FindObjectOfType<RoadRenderer>();
        Services.car = FindObjectOfType<Car>();
        Services.roadsideObjectManager = FindObjectOfType<RoadsideObjectManager>();
    }

    public void Update() {
        drivingTime += Time.deltaTime * drivingTimeScale;
        drivingDeltaTime = drivingTime - lastFrameTime;
        lastFrameTime = drivingTime;
    }

    public void Crash() {
        StartCoroutine(CrashSequence());
    }

    private IEnumerator CrashSequence() {
        drivingTimeScale = 0f;
        foreach (Animator animator in FindObjectsOfType<Animator>()) {
            animator.enabled = false;
        }

        GameObject cracks = GameObject.Find("CRACKS");
        cracks.GetComponent<SpriteRenderer>().enabled = true;
        //cracks.GetComponent<Animator>().enabled = true;

        yield return null;
    }
}
