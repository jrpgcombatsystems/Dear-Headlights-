using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

    [SerializeField] private GameObject[] deerPrefabs;
    [SerializeField] private float deerSpawnDistance = 5f;
    private float distanceSinceLastDeerSpawn = 0f;

    [SerializeField] private GameObject oncomingCarPrefab;
    private float oncomingCarSpawnDistance = 7f;
    private float distanceSinceLastOncomingCarSpawn = 0f;

    [SerializeField] private GameObject sameDirectionCarPrefab;
    private float sameDirectionCarSpawnDistance = 10f;
    private float distanceSinceLastSameDirectionCarSpawn = 0f;

    private void Update() {
        float currentPlayerSpeed = Services.playerCar.currentSpeed * Services.gameManager.drivingDeltaTime;

        // Handle deer spanwing
        distanceSinceLastDeerSpawn += currentPlayerSpeed;
        if (distanceSinceLastDeerSpawn >= deerSpawnDistance) {
            SpawnDeer();
            distanceSinceLastDeerSpawn = 0f;
        }

        distanceSinceLastOncomingCarSpawn += currentPlayerSpeed;
        if (distanceSinceLastOncomingCarSpawn >= oncomingCarSpawnDistance) {
            SpawnOncomingCar();
            distanceSinceLastOncomingCarSpawn = 0f;
        }

        distanceSinceLastSameDirectionCarSpawn += currentPlayerSpeed;
        if (distanceSinceLastSameDirectionCarSpawn >= sameDirectionCarSpawnDistance) {
            SpawnSameDirectionCar();
            distanceSinceLastSameDirectionCarSpawn = 0f;
        }
    }

    private void SpawnDeer() {
        RoadObject newDear = Instantiate(deerPrefabs[Random.Range(0, deerPrefabs.Length)]).GetComponent<RoadObject>();
        newDear.roadPosition = Random.value >= 0.5f ? -1.5f : 1.5f;
    }

    private void SpawnOncomingCar() {
        RoadObject newCar = Instantiate(oncomingCarPrefab).GetComponent<RoadObject>();
        newCar.roadPosition = Random.Range(-0.75f, -0.25f);
    }

    private void SpawnSameDirectionCar() {
        RoadObject newCar = Instantiate(sameDirectionCarPrefab).GetComponent<RoadObject>();
        newCar.roadPosition = Random.Range(0.25f, 0.75f);
    }
}
