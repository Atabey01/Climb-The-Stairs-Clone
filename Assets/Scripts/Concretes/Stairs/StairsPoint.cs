using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsPoint : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _offset;
    private void Start()
    {
        _offset = transform.position - _player.transform.position;
    }
    private void Update()
    {
        transform.position = _offset + _player.transform.position;
    }

}
