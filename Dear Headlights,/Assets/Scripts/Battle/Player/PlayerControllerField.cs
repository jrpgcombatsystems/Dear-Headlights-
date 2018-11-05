using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerField : MonoBehaviour {

    [SerializeField] float speed;

    Rigidbody m_Rigidbody { get { return GetComponent<Rigidbody>(); } }


    public void Run() {
        // Get input and move
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        m_Rigidbody.MovePosition(transform.position + input.normalized * speed * Time.deltaTime);

        // If player is moving, test for a random battle
        if (input != Vector3.zero) { BattleSystem.Services.battleChanceManager.TestForBattle(); }
    }
}
