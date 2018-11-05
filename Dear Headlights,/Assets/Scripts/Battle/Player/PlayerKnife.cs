using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnife : MonoBehaviour {

    [SerializeField] float speed;
    Rigidbody m_Rigidbody { get { return GetComponent<Rigidbody>(); } }


	void Run() {
        m_Rigidbody.MovePosition(transform.position + transform.forward * speed);
    }
}
