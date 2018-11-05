using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy {

    [SerializeField] float accelerationSpeed;
    [SerializeField] float maxSpeed;

    Vector3 velocity;


    public override void Run() {
        base.Run();

        // Move toward player
        Vector3 acceleration = DirectionToPlayer * accelerationSpeed;
        Vector3 velocity = m_Rigidbody.velocity + acceleration;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        m_Rigidbody.velocity = velocity;
    }
}
