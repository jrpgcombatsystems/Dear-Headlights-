using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {

    [SerializeField] float maxScale = 20f;
    public float roadPosition = 0.25f;
    [SerializeField] AnimationCurve approachCurve;
    [SerializeField] float approachSpeed = 0.1f;
    [SerializeField] bool loop;
    [SerializeField] bool isCollidable;

    [HideInInspector] public float currentDistance;

    private Vector3 originalScale;
    private bool deleteTag = false;

    private void Awake() {
        originalScale = transform.localScale;
    }

    Vector3 previousPosition = Vector3.zero;
    private void Update() {
        if (deleteTag) {
            RemoveSelf();
            return;
        }

        currentDistance = Mathf.Clamp01(currentDistance + approachSpeed * Services.gameManager.drivingDeltaTime * Services.car.currentSpeed);

        float yPosition = approachCurve.Evaluate(currentDistance);
        yPosition = Den.Math.Map(yPosition, 0f, 1f, Services.roadRenderer.horizon, Services.roadRenderer.leftEdgeCurve.lowerPoint.y);

        transform.localPosition = Services.roadRenderer.GetRoadPosition(roadPosition, yPosition);

        float newScale = Den.Math.Map(yPosition, Services.roadRenderer.horizon, Services.roadRenderer.leftEdgeCurve.lowerPoint.y, 0.01f, maxScale);
        transform.localScale = originalScale * newScale;

        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(Den.Math.Map(currentDistance, 0f, 1f, 0f, 10000f));

        if (currentDistance >= 0.99) {
            if (isCollidable) {
                // See if we are colliding with the player.
                float viewportX = Camera.main.WorldToViewportPoint(previousPosition).x;
                if (viewportX >= 0.2f && viewportX < 0.7f) {
                    transform.localPosition = previousPosition;
                    isCollidable = false;
                    Services.gameManager.Crash();
                }
            }

            else {
                deleteTag = true;
            }
        }

        previousPosition = transform.localPosition;
    }

    void RemoveSelf() {
        if (loop) {
            currentDistance = 0f;
            roadPosition = Services.roadsideObjectManager.GetRandomOffRoadPosition();
            deleteTag = false;
        }

        else {
             Destroy(gameObject);
        }
    }
}
