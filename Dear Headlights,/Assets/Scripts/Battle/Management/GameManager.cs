using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
    public class GameManager : MonoBehaviour {

        enum GameState { Idle, Battle }
        GameState gameState;


        private void OnEnable() {
            GameEventManager.instance.Subscribe<GameEvents.BattleStarted>(BattleStartedHandler);
            GameEventManager.instance.Subscribe<GameEvents.BattleWon>(BattleWonHandler);
        }


        private void OnDisable() {
            GameEventManager.instance.Unsubscribe<GameEvents.BattleStarted>(BattleStartedHandler);
            GameEventManager.instance.Unsubscribe<GameEvents.BattleWon>(BattleWonHandler);
        }


        private void Awake() {
            Services.playerControllerField = FindObjectOfType<PlayerControllerField>();
            Services.playerControllerBattle = FindObjectOfType<PlayerControllerBattle>();
            Services.battleChanceManager = GetComponentInChildren<BattleChanceManager>();
            Services.battleManager = FindObjectOfType<BattleManager>();
            Services.gameEventManager = new GameEventManager();
        }


        private void Update() {
            if (gameState == GameState.Idle) {
                //Services.playerControllerField.Run();
            }

            else if (gameState == GameState.Battle) {
                //Services.battleManager.Run();
            }
        }


        void BattleStartedHandler(GameEvent gameEvent) {
            gameState = GameState.Battle;
        }


        void BattleWonHandler(GameEvent gameEvent) {
            gameState = GameState.Idle;
        }
    }
}
