using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    public float health;

    [HideInInspector] public bool markForDeletion;

    protected Vector3 DirectionToPlayer { get { return Vector3.Normalize(BattleSystem.Services.playerControllerBattle.transform.position - transform.position); } }

    protected Rigidbody m_Rigidbody { get { return GetComponent<Rigidbody>(); } }


    public virtual void Initialize() { }
    public virtual void Run() {
        if (health <= 0f) { Die(); }
    }
    
    void Die() {
        markForDeletion = true;
    }
}
