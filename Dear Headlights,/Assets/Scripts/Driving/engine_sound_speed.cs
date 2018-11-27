using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engine_sound_speed : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetComponent<AudioSource>().pitch = (Den.Math.Map(Services.car.currentSpeed, 0f, Services.car.maxSpeed, 0.2f, 3f));

	}
}
