using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings", order = 1)]
public class CameraSettings : ScriptableObject
{
    [Range(0.1f, 10f)]
    public float sensitivty;
    public float yMaxAngle;
}
