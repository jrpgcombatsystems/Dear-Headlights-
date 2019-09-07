using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;
using DG.Tweening;

namespace BattleSystem {
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour {

        public float health;

        [HideInInspector] public bool markForDeletion;

        protected Vector3 DirectionToPlayer { get { return Vector3.Normalize(BattleSystem.Services.playerController.transform.position - transform.position); } }
        protected bool isActive = false;

        protected Rigidbody m_Rigidbody;
        protected SphereCollider m_Collider;
        protected GameObject visuals;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<SphereCollider>();
            visuals = GetComponentInChildren<Renderer>().gameObject;
        }

        public virtual void Initialize() {
            isActive = true;
        }

        public virtual void Run() {
            if (!isActive) return;
            if (health <= 0f) { Die(); }
        }

        void Die() {
            markForDeletion = true;
        }

        public Coroutine BeginSpawningSequence() {
            return StartCoroutine(SpawningSequence());
        }

        private IEnumerator SpawningSequence() {
            // Pick a random position, and make sure we're not overlapping anything
            Vector3 spawningPosition = Services.battleManager.GetRandomArenaPosition(m_Collider.radius);
            for (int i = 0; i < 100; i++) {
                Collider[] overlappingColliders = Physics.OverlapSphere(m_Collider.center, m_Collider.radius);
                if (overlappingColliders.Length == 0) {
                    break;
                }
                spawningPosition = Services.battleManager.GetRandomArenaPosition(m_Collider.radius);
            }
            transform.localPosition = spawningPosition;

            // Change transform in preparation for spawning sequence
            Vector3 preSpawnScale = new Vector3(1f, 100f, 0.001f);
            visuals.transform.localScale = preSpawnScale;

            visuals.transform.localRotation = Quaternion.Euler(-540, 0, 90);

            // Scale to correct size
            float duration = 0.5f;
            visuals.transform.DOBlendableLocalRotateBy(new Vector3(360, 0, 0), duration);
            visuals.transform.DOScaleY(1f, duration).SetEase(Ease.InQuint);
            yield return new WaitForSeconds(duration * 0.5f);
            visuals.transform.DOScaleZ(1f, duration * 0.5f);
            yield return new WaitForSeconds(duration * 0.5f);

            // Quick pause
            yield return new WaitForSeconds(0.4f);

            // Rotate to correct position
            duration = 0.25f;
            visuals.transform.DOBlendableLocalRotateBy(new Vector3(90, 0, 0), duration);
            yield return new WaitForSeconds(duration);

            // Quick pause
            yield return new WaitForSeconds(0.4f);

            yield return null;
        }
    }
}
