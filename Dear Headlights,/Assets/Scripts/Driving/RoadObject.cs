using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {

    [SerializeField] float maxScale = 20f;
    public float roadPosition = 0.25f;
    [SerializeField] AnimationCurve approachCurve;
    [SerializeField] float approachSpeed = 0.1f;
    [SerializeField] bool loop;

    [HideInInspector] public float currentDistance;
    Vector3 originalScale;

    private void Awake() {
        originalScale = transform.localScale;
    }

    private void Update() {
        float yPosition = approachCurve.Evaluate(currentDistance);
        yPosition = Den.Math.Map(yPosition, 0f, 1f, Services.roadRenderer.horizon, Services.roadRenderer.leftEdgeCurve.lowerPoint.y);

        transform.position = Services.roadRenderer.GetRoadPosition(roadPosition, yPosition);

        float newScale = Den.Math.Map(yPosition, Services.roadRenderer.horizon, Services.roadRenderer.leftEdgeCurve.lowerPoint.y, 0.01f, maxScale);
        transform.localScale = originalScale * newScale;

        currentDistance += approachSpeed * Time.deltaTime * Services.car.currentSpeed;

        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(Den.Math.Map(currentDistance, 0f, 1f, 0f, 10000f));

        if (currentDistance > 1) {
            RemoveSelf();
        }
    }

    void RemoveSelf() {
        if (loop) {
            currentDistance = 0f;
            roadPosition = Services.roadsideObjectManager.GetRandomOffRoadPosition();
        }

        else {
             Destroy(gameObject);
        }
    }
}
