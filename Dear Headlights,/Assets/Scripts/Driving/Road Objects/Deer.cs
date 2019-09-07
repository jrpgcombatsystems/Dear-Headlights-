using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Den;

public class Deer : RoadObject
{
    [HideInInspector] public float moveDirection = -1;
    private FloatRange moveSpeedRange = new FloatRange(90f, 150);
    private FloatRange jumpOutDistanceRange = new FloatRange(0.6f, 0.8f);

    private float moveSpeed;
    private float jumpOutDistance;
    private bool willFreeze;

    private enum State { WaitingToDart, Darting, Frozen }
    private State state;

    private void Start() {
        moveDirection = roadPosition >= 0 ? -1f : 1f;
        if (moveDirection == 1) { m_SpriteRenderer.flipX = true; }

        jumpOutDistance = jumpOutDistanceRange.Random;
        moveSpeed = moveSpeedRange.Random;
        willFreeze = Random.value <= 0.8 ? true : false;
    }

    protected override void Update() {
        base.Update();

        switch (state) {
            case State.WaitingToDart:
                if (currentDistance >= jumpOutDistance) { state = State.Darting; }
                break;

            case State.Darting:
                roadPosition += moveDirection * moveSpeed * (1 / Services.roadRenderer.roadWidth) * Services.gameManager.drivingDeltaTime;
                if (willFreeze && Mathf.Abs(roadPosition - Services.playerCar.roadPosition) <= 0.1f) {
                    GetComponent<Animator>().enabled = false;
                    state = State.Frozen;
                }
                break;

            case State.Frozen:
                break;
        }

    }
}
