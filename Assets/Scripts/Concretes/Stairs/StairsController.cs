using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimbTheStairs.Stairs
{
    public class StairsController : MonoBehaviour
    {
        Queue<GameObject> _stairsPoolObjects;
        public List<GameObject> StairsList;

        [SerializeField] GameObject _stairsSpawnPoint;
        [SerializeField] GameObject _stairsStartPoint;
        [SerializeField] GameObject _stairsPrefab;
        [SerializeField] float _poolSize;
        [SerializeField] float _stairsSpawnTime;

        [Header("Color")]
        public Material _modelMat;
        public Color _whiteColor = Color.white;
        public Color _redColor = Color.red;
        private void Awake()
        {
            _stairsPoolObjects = new Queue<GameObject>(); // Creating a Queue for Object Pooling
            _modelMat = Resources.Load<Material>("Materials/TimesRunningOut");

            StairsList = new List<GameObject>(); // Adding Queue To List
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject stairs = Instantiate(_stairsPrefab);
                stairs.SetActive(false);
                _stairsPoolObjects.Enqueue(stairs);
                StairsList.Add(stairs);
            }
        }
        public GameObject GetPooledObject()
        {
            GameObject stair = _stairsPoolObjects.Dequeue();
            stair.SetActive(true);
            _stairsPoolObjects.Enqueue(stair);
            return stair;
        }
        public void StairsRoutine()
        {
            
            var Stairs = GetPooledObject();
            Stairs.transform.localScale = Vector3.zero;
            Stairs.transform.DOScale(Vector3.one, 0.2f);
            Stairs.transform.DOMove(_stairsStartPoint.transform.position, 0.2f);
            Stairs.transform.position = _stairsSpawnPoint.transform.position;
            Stairs.transform.rotation = _stairsSpawnPoint.transform.rotation;
        }
    }
}

