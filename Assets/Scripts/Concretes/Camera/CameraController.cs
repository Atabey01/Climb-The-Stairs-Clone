using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbTheStairs.Concretes
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] CameraSettings _cameraSettings;
        private void Update()
        {
            Vector3 _cameraPosition = new Vector3(0, _target.position.y, 0) + _cameraSettings.CameraOffset;
            Vector3 _cameraSmoothPosition = Vector3.Lerp(transform.position, _cameraPosition, _cameraSettings.CameraLerpSpeed * Time.deltaTime);

            transform.position = _cameraSmoothPosition;

        }

    }
}
