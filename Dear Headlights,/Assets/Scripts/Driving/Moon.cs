using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

    [SerializeField] float moveSpeed = 0.5f;

    private void Update() {
        Vector3 newPosition = transform.position;
        newPosition.x += (Services.car.turningValue - Services.car.steeringInfluenceFromCurve) * -moveSpeed * Services.gameManager.drivingDeltaTime;
        newPosition.x = Den.Math.Wrap(newPosition.x, -15f, 15f);
        transform.position = newPosition;
    }
}
