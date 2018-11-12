using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores static references to various important objects so that scripts across the game can have access to them.
/// </summary>
public class Services {

    public static GameManager gameManager;
    public static RoadManager roadManager;
    public static RoadRenderer roadRenderer;
    public static Car car;
}
