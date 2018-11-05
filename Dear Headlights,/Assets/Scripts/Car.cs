using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    [SerializeField] float turnSpeed = 1f;

    private void Update() {
        Services.roadRenderer.lowerOffset += Input.GetAxis("Horizontal") * -turnSpeed * Time.deltaTime;
    }
}
