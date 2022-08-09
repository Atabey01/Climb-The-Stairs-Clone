using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Settings", menuName = "ScriptableObject/Camera/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [SerializeField] Vector3 _cameraOffset;
    [SerializeField] float _cameraLerpSpeed;

    public Vector3 CameraOffset { get => _cameraOffset; private set => _cameraOffset = value; }
    public float CameraLerpSpeed { get => _cameraLerpSpeed; private set => _cameraLerpSpeed = value; }
}
