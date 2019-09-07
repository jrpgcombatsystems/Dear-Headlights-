using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class PlayerGun : MonoBehaviour {

    [SerializeField] GameObject knifePrefab;
    [SerializeField] float shotFrequency = 0.1f;

    [HideInInspector] public Enemy currentTarget;

    private float shotTimer = 0f;

    private void Awake() {
        shotTimer = shotFrequency;
    }

    public void Fire() {    
        // Get nearest enemy.
        Enemy nearestEnemy = null;
        float distanceToNearestEnemy = 999999999f;
        foreach (Enemy enemy in BattleSystem.Services.battleManager.enemies) {
            float distanceToThisEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToThisEnemy < distanceToNearestEnemy) {
                nearestEnemy = enemy;
                distanceToNearestEnemy = distanceToThisEnemy;
            }
        }

        if (nearestEnemy == null) {
            Debug.Log("Player gun could not find nearest enemy.");
            return;
        }

        currentTarget = nearestEnemy;

        // Shoot
        shotTimer += Time.deltaTime;
        if (shotTimer >= shotFrequency) {
            Shoot();
            shotTimer = 0;
        }
    }

    void Shoot() {
        PlayerKnife newKnife = Instantiate(knifePrefab).GetComponent<PlayerKnife>();
        newKnife.Initialize(transform.position, Vector3.Normalize(transform.forward));
    }
}
