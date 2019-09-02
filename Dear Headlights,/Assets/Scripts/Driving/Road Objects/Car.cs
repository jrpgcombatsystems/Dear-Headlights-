using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Den;

public class Car : RoadObject
{
    [SerializeField] private FloatRange approachSpeedRange = new FloatRange(0.4f, 0.6f);

    private void Start() {
        m_Data.approachSpeed = approachSpeedRange.Random;
    }
}
