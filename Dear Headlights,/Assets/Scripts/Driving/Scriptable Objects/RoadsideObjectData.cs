using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data Containers", menuName = "Roadside Object Data", order = 51)]
public class RoadsideObjectData : ScriptableObject
{
    // The scale of the object when it is at the same vertical position as the car.
    public float maxScale = 1.5f;

    // How quickly the object approaches the car
    public float approachSpeed = 0.1f;

    // Whether the object loops back to the horizon when it reaches the player's position
    public bool loop;

    // Whether the player can crash into this object
    public bool isCollidable;

    // Whether this sprite has it's color modified as it gets closer to the screen
    public bool modifyColorWithDistance = false;
}
