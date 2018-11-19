using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private void Awake() {
        // Locate and store references to various important classes.
        Services.gameManager = this;
        Services.roadManager = FindObjectOfType<RoadManager>();
        Services.roadRenderer = FindObjectOfType<RoadRenderer>();
        Services.car = FindObjectOfType<Car>();
        Services.roadsideObjectManager = FindObjectOfType<RoadsideObjectManager>();
    }

    public void Crash() {
        Debug.Log("you fucking died");
        // This'll do more stuff later
    }
}
