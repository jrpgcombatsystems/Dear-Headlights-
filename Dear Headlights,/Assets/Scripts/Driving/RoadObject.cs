﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {

    [SerializeField] float maxScale = 20f;
    [SerializeField] float roadPosition = 0.25f;
    [SerializeField] AnimationCurve approachCurve;

    [SerializeField] float currentDistance;
    [SerializeField] float approachSpeed = 0.1f;

    [SerializeField] bool randomizePosition = false;

    Vector3 originalScale;

    private void Awake() {
        originalScale = transform.localScale;
    }

    private void Update() {
        float yPosition = approachCurve.Evaluate(currentDistance);
        yPosition = MyMath.Map(yPosition, 0f, 1f, Services.roadRenderer.horizon, Services.roadRenderer.leftRoad.endPoint.position.y);

        transform.position = Services.roadRenderer.GetRoadPosition(roadPosition, approachCurve.Evaluate(currentDistance));

        float newScale = MyMath.Map(yPosition, Services.roadRenderer.horizon, Services.roadRenderer.leftRoad.endPoint.position.y, 0.01f, maxScale);
        transform.localScale = originalScale * newScale;

        currentDistance += approachSpeed * Time.deltaTime * Services.gameManager.currentSpeed;

        if (currentDistance > 1) {
            currentDistance = 0;
            if (randomizePosition) { roadPosition = Random.value; }
        }
    }
}