using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerBattle : MonoBehaviour {

    [SerializeField] float speed;

    Rigidbody m_Rigidbody { get { return GetComponent<Rigidbody>(); } }
    PlayerGun m_Gun { get { return GetComponentInChildren<PlayerGun>(); } }


    public void Run() {
        // Get input and move
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        m_Rigidbody.velocity = input.normalized * speed * Time.deltaTime;
        m_Gun.Fire();
    }
}
