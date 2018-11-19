using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSpawner : MonoBehaviour {

    [SerializeField] private GameObject[] deerPrefabs;
    [SerializeField] private float spawnFrequency = 1f;

    private float spawnTimer = 0f;

    private void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnFrequency) {

            RoadObject newDear = Instantiate(deerPrefabs[Random.Range(0, deerPrefabs.Length)]).GetComponent<RoadObject>();
            newDear.roadPosition = Random.Range(-0.9f, 0.9f);

            spawnTimer = 0f;
        }
    }
}
