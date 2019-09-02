using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

    [SerializeField] float moveSpeed = 0.5f;

    private void Update() {
        Vector3 newPosition = transform.localPosition;
        newPosition.x += (Services.playerCar.turningValue - Services.playerCar.steeringInfluenceFromCurve) * (1/Services.roadRenderer.roadWidth) * Services.gameManager.drivingDeltaTime * -1;
        newPosition.x = Den.Math.Wrap(newPosition.x, -15f, 15f);
        transform.localPosition = newPosition;
    }
}
