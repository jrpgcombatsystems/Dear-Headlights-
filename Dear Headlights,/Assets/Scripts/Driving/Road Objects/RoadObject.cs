using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {

    [SerializeField] RoadsideObjectData _m_Data;
    protected RoadsideObjectData m_Data;

    // The object's horizontal position on the road, represented as a percentage from the left side of the road to the right.
    [HideInInspector] public float roadPosition = 0.25f;    
    
    // The object's vertical position on the road, represented as a percentage where 0 is the horizon and 1 is the car's position.
    [HideInInspector] public float currentDistance;

    private Vector3 originalScale;
    private bool deleteTag = false;

    private const float MAX_DISTANCE = 0.985f;

    protected SpriteRenderer m_SpriteRenderer;

    private void Awake() {
        originalScale = transform.localScale;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Data = Instantiate(_m_Data);
    }

    Vector3 previousPosition = Vector3.zero;

    protected virtual void Update() 
    {
        if (deleteTag) {
            RemoveSelf();
            return;
        }

        currentDistance = Mathf.Clamp01(currentDistance + m_Data.approachSpeed * Services.gameManager.drivingDeltaTime * Services.playerCar.currentSpeed);

        float yPosition = Services.gameManager.approachCurve.Evaluate(currentDistance);
        yPosition = Den.Math.Map(yPosition, 0f, 1f, Services.roadRenderer.horizon, Services.roadRenderer.leftEdgeCurve.lowerPoint.y);

        transform.localPosition = Services.roadRenderer.GetRoadPosition(roadPosition, yPosition);

        float newScale = Den.Math.Map(yPosition, Services.roadRenderer.horizon, Services.roadRenderer.leftEdgeCurve.lowerPoint.y, 0.01f, m_Data.maxScale);
        transform.localScale = originalScale * newScale;

        if (m_SpriteRenderer) {
            m_SpriteRenderer.sortingOrder = Mathf.FloorToInt(Den.Math.Map(currentDistance, 0f, 1f, 0f, 10000f));

            if (m_Data.modifyColorWithDistance) {
                float newValue = Services.gameManager.approachCurve.Evaluate(currentDistance);
                newValue = Den.Math.Map(newValue, 0, 0.5f, 0, 1);
                m_SpriteRenderer.color = new Color(newValue, newValue, newValue);
            }
        }

        if (currentDistance >= MAX_DISTANCE) {
            if (m_Data.isCollidable) {
                // Check for collision with player
                BoxCollider2D m_Collider = GetComponent<BoxCollider2D>();
                Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(m_Collider.bounds.center, m_Collider.size, 0f);
                foreach(Collider2D collider in overlappingColliders) {
                    if (collider.GetComponent<PlayerCar>()) {
                        transform.position = previousPosition;
                        currentDistance = MAX_DISTANCE - 0.001f;
                        m_Data.isCollidable = false;
                        Services.gameManager.Crash();
                    }
                }
            }

            else {
                deleteTag = true;
            }
        }

        previousPosition = transform.position;
    }

    void RemoveSelf() {
        if (m_Data.loop) {
            currentDistance = currentDistance - MAX_DISTANCE;
            roadPosition = Services.roadsideObjectManager.GetRandomOffRoadPosition();
            deleteTag = false;
        }

        else {
             Destroy(gameObject);
        }
    }
}
