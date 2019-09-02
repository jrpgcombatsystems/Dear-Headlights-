using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores car-related variables such as speed, and also gets the player's input for driving.
/// </summary>
public class PlayerCar : MonoBehaviour {

    [SerializeField] private float speedAtGameStart = 1f;
    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private float minTurnSpeed = 0.1f;
    [SerializeField] private float accelerationSpeed = 0.05f;
    [SerializeField] private float decelerationSpeed = 0.4f;
    [SerializeField] private float maxBrakingMultiplier = 0.4f;
    [SerializeField] private float maxSpeed = 1000f;
    [SerializeField] private float invincibilityFrameTime = 1f;

    public float currentSpeed { get { return _currentSpeed * (isBraking ? brakingModifier : 1); } }
    [HideInInspector] public float turningValue = 0f; // How much the player is currently steering in a given direction. Should range from -1 to 1.
    [HideInInspector] public float roadPosition = 0.5f; // The player's position on the road. Ranges from -1 to 1 where the -1 is all the way left and 1 is all the way to the right.
    [HideInInspector] public float steeringInfluenceFromCurve = 0f; // How much our turning is being influnced by a curve in the road.
    [HideInInspector] public float distanceTravelled = 0f;

    private float _currentSpeed = 0f;
    private float previousRoadPosition;
    private float invincibilityFrameTimer = 0f;
    private bool isBraking = false;
    private float skidSpeed;
    private float brakingValue = 0f;
    private float brakingModifier = 1f;

    private void Awake() {
        _currentSpeed = speedAtGameStart;
        previousRoadPosition = turningValue;
    }

    private void Update() {
        // Accelerate.
        if (Input.GetAxisRaw("Vertical") > 0) {
            _currentSpeed += accelerationSpeed * Services.gameManager.drivingDeltaTime;
        }

        // Decelerate
        else {
            _currentSpeed -= decelerationSpeed * Services.gameManager.drivingDeltaTime;
        }

        // Clamp current speed
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);

        // Handle braking
        if (Input.GetAxisRaw("Vertical") < 0) {
            brakingValue = Mathf.Clamp01(Mathf.Lerp(brakingValue, 1f, 2.5f * Services.gameManager.drivingDeltaTime));

            // If this is the first frame we're braking, save the skid speed
            if (!isBraking) {
                skidSpeed = Den.Math.Map(currentSpeed, 0f, maxSpeed, 1f, 2f);
                isBraking = true;
            }
        }
        else {
            isBraking = false;
            brakingValue = Mathf.Clamp01(brakingValue - 1f * Services.gameManager.drivingDeltaTime);
        }

        brakingModifier = Den.Math.Map(brakingValue, 0f, 1f, 1f, maxBrakingMultiplier);

        // Get horizontal input.
        turningValue = Input.GetAxis("Horizontal") * turnSpeed * (currentSpeed + minTurnSpeed) * (1/Services.roadRenderer.roadWidth) * Services.gameManager.drivingDeltaTime;

        // Apply current turn influence.
        turningValue += steeringInfluenceFromCurve * currentSpeed * (1 / Services.roadRenderer.roadWidth) * Services.gameManager.drivingDeltaTime;

        // Apply skidding value from braking
        if (isBraking) {
            turningValue *= skidSpeed;
            skidSpeed = Mathf.Lerp(skidSpeed, 0, 0.05f * Services.gameManager.drivingDeltaTime);
            Debug.Log("skid speed: " + skidSpeed);
        }

        roadPosition += turningValue;

        distanceTravelled += currentSpeed * Services.gameManager.drivingDeltaTime;

        invincibilityFrameTimer += Services.gameManager.drivingDeltaTime;

        // See if the player went off the side of the road.
        if (invincibilityFrameTimer >= invincibilityFrameTime)
        if (roadPosition < -1 || roadPosition > 1) {
            roadPosition = previousRoadPosition;
            Services.gameManager.Crash();
        }

        previousRoadPosition = roadPosition;
    }
}
