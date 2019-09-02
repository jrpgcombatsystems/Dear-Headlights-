using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This stores references to object prefabs which are in a group. For example, you we might have a 'corn group' or a 'suburb group' or a 'forest group'. Then, within that group
/// you can decide the likelihood that a certain item will appear. So we could have scarecrows in the corn group that appear much less frequently than the various cornstalk sprites.
/// </summary>
public class RoadsideObjectGroup : MonoBehaviour {

    public float objectSpawnFrequency = 0.5f; // How often an object from this group spawns. Affects the density of objects in this group.
    [SerializeField] private RoadsideObject[] roadsideObjects;

    private GameObject[] weightedArray;

    [Serializable]
    private class RoadsideObject {
        public GameObject objectPrefab;
        public int appearanceWeight = 1;    // Affects the likelihood that this item will actually appear.
    }


    private void Awake() {
        int weightedArrayLength = 0;
        for (int i = 0; i < roadsideObjects.Length; i++) {
            weightedArrayLength += roadsideObjects[i].appearanceWeight;
        }

        weightedArray = new GameObject[weightedArrayLength];
        int weightedArrayIndex = 0;

        for (int i = 0; i < roadsideObjects.Length; i++) {
            for (int j = 0; j < roadsideObjects[i].appearanceWeight; j++) {
                weightedArray[weightedArrayIndex] = roadsideObjects[i].objectPrefab;
                weightedArrayIndex++;
            }
        }
    }


    public GameObject GetRandomObject() {
        return weightedArray[UnityEngine.Random.Range(0, weightedArray.Length)];
    }
}
