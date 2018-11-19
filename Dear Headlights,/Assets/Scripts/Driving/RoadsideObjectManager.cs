using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for placing objects outside the road.
/// </summary>
public class RoadsideObjectManager : MonoBehaviour {

    [SerializeField] float maxDistanceFromRoad = 10f;
    [SerializeField] float minDistanceFromRoad = 0.5f;
    [SerializeField] int numberOfObjects;

    private RoadsideObjectGroup[] objectGroups;
    private RoadsideObjectGroup currentObjectGroup;

    private void Awake() {
        objectGroups = GetComponentsInChildren<RoadsideObjectGroup>();

        // This script should include functionality for choosing a new object group, but since there's only one right now, don't bother.
        currentObjectGroup = objectGroups[0];
    }

    private void Start() {
        for (int i = 0; i < numberOfObjects; i++) {
            SpawnObject();
        }
    }

    private void SpawnObject() {
        GameObject newObject = Instantiate(currentObjectGroup.GetRandomObject());
        newObject.GetComponent<RoadObject>().roadPosition = GetRandomOffRoadPosition();
        newObject.GetComponent<RoadObject>().currentDistance = Random.value;
    }

    public float GetRandomOffRoadPosition() {
        // Decide whether this object will be on the left or right side of the road
        int leftOrRight = -1;
        if (Random.value >= 0.5f) { leftOrRight = 1; }
        return leftOrRight * Random.Range(1f + minDistanceFromRoad, maxDistanceFromRoad + 1);
    }
}
