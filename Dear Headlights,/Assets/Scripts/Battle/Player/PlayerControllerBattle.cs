using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerBattle : MonoBehaviour {

    [SerializeField] float speed;

    Rigidbody m_Rigidbody { get { return GetComponent<Rigidbody>(); } }
    PlayerGun m_Gun { get { return GetComponentInChildren<PlayerGun>(); } }

    Vector3 input = Vector3.zero;

    public void Run() {
        // Get input and move
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        m_Gun.Fire();
    }

    public void FixedRun() {
        m_Rigidbody.MovePosition(transform.position + input.normalized * speed * Time.fixedDeltaTime);
    }
}
