using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for spawning enemies and running their behavior routines, as well as for seeing whether all the enemies have been defeated, and ending the battle sequence accordingly.
/// </summary>
public class BattleManager : MonoBehaviour {

    [SerializeField] Den.IntRange enemyAmountRange = new Den.IntRange(2, 5);

    // The size, in world units of the battle screen.
    [SerializeField] Vector2 arenaSize;

    [SerializeField] PlayerControllerBattle player;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Camera battleCamera;

    // Contains a reference to all the enemies currently alive.
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    bool isInitialized = false;

    
    public void OnEnable() {
        GameEventManager.instance.Subscribe<GameEvents.BattleStarted>(BattleStartedHandler);
        GameEventManager.instance.Subscribe<GameEvents.BattleWon>(BattleWonHandler);
    }


    private void OnDisable() {
        GameEventManager.instance.Unsubscribe<GameEvents.BattleStarted>(BattleStartedHandler);
        GameEventManager.instance.Unsubscribe<GameEvents.BattleWon>(BattleWonHandler);
    }

    public void Awake() {
        BattleSystem.Services.battleManager = this;
        BattleSystem.Services.playerControllerBattle = FindObjectOfType<PlayerControllerBattle>();
        BattleSystem.Services.playerControllerBattle.gameObject.SetActive(false);
    }
    
    public void Update() {
        if (!isInitialized) { return; }

        // Check to see if the player has defeated all the enemies. If so, end the battle.
        if (enemies.Count == 0) {
            battleCamera.enabled = false;
            GameEventManager.instance.FireEvent(new GameEvents.BattleWon());
        }

        for (int i = enemies.Count-1; i >= 0; i--) {
            if (enemies[i].markForDeletion) {
                Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
            }
            else { enemies[i].Run(); }
        }

        player.Run();
    }

    private void FixedUpdate() {
        player.FixedRun();
    }

    void BattleStartedHandler(GameEvent gameEvent) {
        // Activate player
        BattleSystem.Services.playerControllerBattle.gameObject.SetActive(true);

        // Spawn enemies
        int numberOfEnemies = enemyAmountRange.Random;
        for (int i = 0; i < numberOfEnemies; i++) {
            Vector3 newEnemyPosition = new Vector3(Random.Range(-arenaSize.x * 0.5f, arenaSize.x * 0.5f), 0f, Random.Range(-arenaSize.y * 0.5f, arenaSize.y * 0.5f));
            Enemy newEnemy = Instantiate(enemyPrefab, newEnemyPosition, Quaternion.identity, transform).GetComponent<Enemy>();
            newEnemy.Initialize();
            enemies.Add(newEnemy);
        }

        // Turn on camera
        battleCamera.enabled = true;

        isInitialized = true;
    }

    void BattleWonHandler(GameEvent gameEvent) {
        isInitialized = false;
    }
}
