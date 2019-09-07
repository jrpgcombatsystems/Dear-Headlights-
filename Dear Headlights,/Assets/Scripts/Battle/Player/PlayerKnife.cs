using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
    public class PlayerKnife : MonoBehaviour {

        [SerializeField] float speed;

        private Vector3 direction;

        Rigidbody m_Rigidbody;
        GameObject visuals;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody>();
            visuals = GetComponentInChildren<MeshRenderer>().gameObject;
        }

        public void Initialize(Vector3 startingPosition, Vector3 direction) {
            transform.position = startingPosition;
            this.direction = direction;
            transform.LookAt(transform.position + direction);
            Physics.IgnoreCollision(GetComponent<Collider>(), Services.playerController.GetComponent<Collider>());
        }

        private void Update() {
            // Rotate
            visuals.transform.Rotate(Vector3.right, 1000f * Time.deltaTime, Space.Self);
        }

        void FixedUpdate() {
            // Move
            m_Rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }
}
