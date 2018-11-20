using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores car-related variables such as speed, and also gets the player's input for driving.
/// </summary>
public class Car : MonoBehaviour {

    public float speedAtGameStart = 1f;
    public float turnSpeed = 20f;
    public float minTurnSpeed = 0.1f;
    public float accelerationSpeed = 0.05f;

    [HideInInspector] public float currentSpeed = 0f;
    [HideInInspector] public float turningValue = 0f; // How much the player is currently steering in a given direction. Should range from -1 to 1.
    [HideInInspector] public float roadPosition = 0.5f; // The player's position on the road. Ranges from -1 to 1 where the -1 is all the way left and 1 is all the way to the right.
    [HideInInspector] public float steeringInfluenceFromCurve = 0f; // How much our turning is being influnced by a curve in the road.

    private float previousRoadPosition;

    private void Awake() {
        currentSpeed = speedAtGameStart;
        previousRoadPosition = turningValue;
    }

    private void Update() {
        // Get horizontal input.
        turningValue = Input.GetAxisRaw("Horizontal") * turnSpeed * (currentSpeed + minTurnSpeed) * Services.gameManager.drivingDeltaTime;

        // Apply current turn influence.
        turningValue += steeringInfluenceFromCurve * Services.car.currentSpeed * Services.gameManager.drivingDeltaTime;

        roadPosition += turningValue;

        // Accelerate.
        if (Input.GetAxisRaw("Vertical") > 0) {
            currentSpeed += accelerationSpeed * Services.gameManager.drivingDeltaTime;
        }

        // See if the player went off the side of the road.
        if (roadPosition < -1 || roadPosition > 1) {
            roadPosition = previousRoadPosition;
            Services.gameManager.Crash();
        }

        previousRoadPosition = roadPosition;
    }
}
