using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores car-related variables such as speed, and also gets the player's input for driving.
/// </summary>
public class Car : MonoBehaviour {

    public float speedAtGameStart = 1f;
    public float turnSpeed = 20f;
    public float accelerationSpeed = 0.05f;

    [HideInInspector] public float currentSpeed = 0f;
    [HideInInspector] public float turningValue = 0f; // How much the player is currently steering in a given direction. Should range from -1 to 1.
    [HideInInspector] public float steeringInfluenceFromCurve = 0f; // How much our turning is being influnced by a curve in the road.

    private void Awake() {
        currentSpeed = speedAtGameStart;
    }

    private void Update() {
        // Get horizontal input.
        turningValue = Input.GetAxisRaw("Horizontal") * turnSpeed * currentSpeed * Time.deltaTime;

        // Apply current turn influence.
        turningValue += steeringInfluenceFromCurve * Services.car.currentSpeed * Time.deltaTime;

        // Accelerate.
        if (Input.GetAxisRaw("Vertical") > 0) {
            currentSpeed += accelerationSpeed * Time.deltaTime;
        }
    }
}
