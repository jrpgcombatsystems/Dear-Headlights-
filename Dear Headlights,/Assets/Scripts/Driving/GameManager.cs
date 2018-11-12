using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float currentSpeed = 1f;

    private void Awake() {
        Services.gameManager = this;
        Services.roadManager = FindObjectOfType<RoadManager>();
        Services.roadRenderer = FindObjectOfType<Road>();
    }

    public void Crash() {
        Debug.Log("you fucking died");
    }
}
