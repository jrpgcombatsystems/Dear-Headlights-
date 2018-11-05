using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class PlayerGun : MonoBehaviour {

    float damageRate = 2f;

    [SerializeField] GameObject knifePrefab;

    LineRenderer m_LineRenderer { get { return GetComponentInChildren<LineRenderer>(); } }

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

        // Draw line to nearest enemy
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, nearestEnemy.transform.position);

        // Damage enemy.
        nearestEnemy.health -= damageRate * Time.deltaTime;
    }
}
