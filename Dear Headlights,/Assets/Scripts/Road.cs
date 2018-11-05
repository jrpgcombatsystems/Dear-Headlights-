using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    public float upperOffset = 0f;
    public float width = 8f;
    public float lowerOffset = 0f;
    public float currentCarInfluence = 0f;

    [SerializeField] LineRenderer leftRoad;
    [SerializeField] LineRenderer rightRoad;

    private void Update() {
        // Set vanishing point
        Vector3 newVanishingPoint = new Vector3(upperOffset, 0f, 0f);
        leftRoad.SetPosition(leftRoad.positionCount - 1, newVanishingPoint);
        rightRoad.SetPosition(rightRoad.positionCount - 1, newVanishingPoint);

        lowerOffset += currentCarInfluence * Time.deltaTime;

        // Set road width
        leftRoad.SetPosition(0, new Vector3((width * -0.5f) + lowerOffset, leftRoad.GetPosition(0).y, 0f));
        rightRoad.SetPosition(0, new Vector3((width * 0.5f) + lowerOffset, rightRoad.GetPosition(0).y, 0f));
    }
}
