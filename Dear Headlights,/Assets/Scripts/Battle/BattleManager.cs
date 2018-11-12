using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    [SerializeField] IntRange enemyAmountRange = new IntRange(2, 5);
    [SerializeField] Vector2 arenaSize;

    [SerializeField] PlayerControllerBattle player;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Camera battleCamera;

    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    
    private void OnEnable() {
        GameEventManager.instance.Subscribe<GameEvents.BattleStarted>(BattleStartedHandler);
    }


    private void OnDisable() {
        GameEventManager.instance.Unsubscribe<GameEvents.BattleStarted>(BattleStartedHandler);
    }


    public void Run() {
        // Check if battle is over.
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


    void BattleStartedHandler(GameEvent gameEvent) {
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
    }
}
