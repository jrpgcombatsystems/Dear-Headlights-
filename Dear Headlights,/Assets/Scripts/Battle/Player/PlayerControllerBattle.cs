using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerBattle : MonoBehaviour {

    [SerializeField] float speed;

    Rigidbody m_Rigidbody;
    PlayerGun m_Gun;
    GameObject visuals;

    Vector3 input = Vector3.zero;
    Vector3 visualsBasePosition;
    Quaternion visualsBaseRotation;

    private void Awake() {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Gun = GetComponentInChildren<PlayerGun>();
        visuals = GetComponentInChildren<MeshRenderer>().gameObject;
        visualsBasePosition = visuals.transform.localPosition;
        visualsBaseRotation = visuals.transform.localRotation;
    }

    public void Run() {
        // Get input
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        m_Gun.Fire();

        // Look at current target
        Vector3 directionToTarget = Vector3.Normalize(m_Gun.currentTarget.transform.position - transform.position);
        directionToTarget.y = 0;
        transform.LookAt(transform.position + directionToTarget);
    }

    public void FixedRun() {
        m_Rigidbody.MovePosition(transform.position + input.normalized * speed * Time.fixedDeltaTime);
    }

    public Coroutine BeginSpawningSequence() {
        return StartCoroutine(SpawningSequence());
    }

    private IEnumerator SpawningSequence() {
        // It'll just be like robotron for now, where the player will always spawn at the center of the arena.
        Vector3 spawningPosition = new Vector3(0f, 0f, transform.localPosition.z);
        transform.localPosition = spawningPosition;

        // Change transform in preparation for spawning sequence
        Vector3 preSpawnScale = new Vector3(1f, 100f, 0.001f);
        visuals.transform.localScale = preSpawnScale;

        visuals.transform.localRotation = Quaternion.Euler(-540, 0, 90);

        // Scale to correct size
        float duration = 1.4f;
        visuals.transform.DOBlendableLocalRotateBy(new Vector3(360, 0, 0), duration);
        visuals.transform.DOScaleY(1f, duration).SetEase(Ease.InQuint);
        yield return new WaitForSeconds(duration * 0.5f);
        visuals.transform.DOScaleZ(1f, duration * 0.5f);
        yield return new WaitForSeconds(duration * 0.5f);

        // Quick pause
        yield return new WaitForSeconds(0.4f);

        // Rotate to correct position
        duration = 0.5f;
        visuals.transform.DOBlendableLocalRotateBy(new Vector3(90, 0, 0), duration);
        yield return new WaitForSeconds(duration);

        // Quick pause
        yield return new WaitForSeconds(0.4f);

        yield return null;
    }
}
