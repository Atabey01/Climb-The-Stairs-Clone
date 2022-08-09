using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ClimbTheStairs.Managers;
using TMPro;

namespace ClimbTheStairs.Stairs
{
    public class PointsController : MonoBehaviour
    {
        [SerializeField] GameObject _stairsPoint;
        [SerializeField] GameObject _stairsPrefab;
        [SerializeField] float _poolSize;
        Queue<GameObject> _stairsPointPoolObjects;
        TextMeshProUGUI _stairsPointText;
        public TextMeshProUGUI _goldText;
        private void Awake()
        {
            _stairsPointPoolObjects = new Queue<GameObject>(); // Creating a Queue for Object Pooling
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject stairs = Instantiate(_stairsPrefab);
                stairs.SetActive(false);
                _stairsPointPoolObjects.Enqueue(stairs);
            }
        }
        private void Update()
        {
            _goldText.text = "" + GameManager.Instance.Gold;
        }
        public GameObject GetPooledObject()
        {
            GameObject stairPoint = _stairsPointPoolObjects.Dequeue();
            stairPoint.SetActive(true);
            _stairsPointPoolObjects.Enqueue(stairPoint);
            return stairPoint;
        }
        public IEnumerator PointsRoutine()
        {
            GameManager.Instance.Gold += GameManager.Instance.GoldIncreaseAmount;
            _goldText.text = "" + GameManager.Instance.Gold;
            
            var Stairs = GetPooledObject();
           
            _stairsPointText = Stairs.GetComponentInChildren<TextMeshProUGUI>();
           
            _stairsPointText.text = "$" + GameManager.Instance.GoldIncreaseAmount;
            
            Stairs.transform.localScale = Vector3.one * 0.003f;
            Stairs.transform.position = _stairsPoint.transform.position;
           
            yield return new WaitForSeconds(0.3f);
            
            Stairs.transform.DOScale(Vector3.zero, 0.5f);
        }

    }
}
