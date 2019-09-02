using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script just turns the steering wheel when the player steers.
/// </summary>
public class SteeringWheel : MonoBehaviour {

    [SerializeField] private float maxRotation = 50f;
    [SerializeField] private float rotateSpeed = 5f;


    private void Update() {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, Den.Math.Map(Input.GetAxis("Horizontal"), -1f, 1f, maxRotation, -maxRotation));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotateSpeed * Services.gameManager.drivingDeltaTime);
    }
}
