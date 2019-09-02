using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engine_sound_speed : MonoBehaviour {

	void Update () {
		GetComponent<AudioSource>().pitch = (Den.Math.Map(Services.playerCar.currentSpeed, 0f, 10f, 0.2f, 3f));
	}
}
