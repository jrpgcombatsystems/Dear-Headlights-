using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [HideInInspector] public float drivingTime;
    public float drivingTimeScale = 1;
    [HideInInspector] public float drivingDeltaTime = 0f;
    [SerializeField] string battleSceneName = "Battle Test 2";

    private float lastFrameTime = 0f;

    private void OnEnable() {
        GameEventManager.instance.Subscribe<GameEvents.BattleWon>(BattleWonHandler);
    }

    private void OnDisable() {
        GameEventManager.instance.Unsubscribe<GameEvents.BattleWon>(BattleWonHandler);
    }


    private void Awake() {
        // Locate and store references to various important classes.
        Services.gameManager = this;
        Services.roadManager = FindObjectOfType<RoadManager>();
        Services.roadRenderer = FindObjectOfType<RoadRenderer>();
        Services.car = FindObjectOfType<Car>();
        Services.roadsideObjectManager = FindObjectOfType<RoadsideObjectManager>();
        Services.timeVortexManager = FindObjectOfType<TimeVortexManager>();
    }

    public void Update() {
        drivingTime += Time.deltaTime * drivingTimeScale;
        drivingDeltaTime = drivingTime - lastFrameTime;
        lastFrameTime = drivingTime;
    }

    public void Crash() {
        StartCoroutine(CrashSequence());
    }

    private IEnumerator CrashSequence() {
        drivingTimeScale = 0f;
        foreach (Animator animator in FindObjectsOfType<Animator>()) {
            animator.enabled = false;
        }

        // Replace with like um crack manager or something
        GameObject cracks = GameObject.Find("CRACKS");
        cracks.GetComponent<SpriteRenderer>().enabled = true;

        Services.timeVortexManager.GetReadyToStart();

        yield return new WaitForSeconds(1f);

        Services.timeVortexManager.BeginVortex();

        // Load battle scene
        SceneManager.LoadScene(battleSceneName, LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();

        // Give battle scene an extra frame to subscribe to events.
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(1f);

        GameEventManager.instance.FireEvent(new GameEvents.BattleStarted());

        yield return null;
    }

    void BattleWonHandler(GameEvent gameEvent) {
        Debug.Log("endo bat");
        StartCoroutine(BattleEndSequence());
    }

    IEnumerator BattleEndSequence() {
        yield return new WaitForSeconds(0.8f);

        SceneManager.UnloadSceneAsync(battleSceneName);

        GameObject cracks = GameObject.Find("CRACKS");
        cracks.GetComponent<Animator>().enabled = true;
        Services.timeVortexManager.GetReadyToEnd();

        // Wait for crack animation to complete, just do this in a dumb way for now.
        yield return new WaitForSeconds(0.2f);

        Services.timeVortexManager.EndVortex();

        foreach (Animator animator in FindObjectsOfType<Animator>()) {
            animator.enabled = true;
        }

        cracks.GetComponent<Animator>().enabled = false;
        cracks.GetComponent<SpriteRenderer>().enabled = false;

        // Reset time scale.
        drivingTimeScale = 1f;

        yield return null;
    }
}
