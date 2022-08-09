using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using ClimbTheStairs.Stairs;
using ClimbTheStairs.Managers;
using ClimbTheStairs.Player;

namespace ClimbTheStairs.Concretes
{
    public class SaveScore : MonoBehaviour
    {
        [Header("Score")]
        [SerializeField] float _currentYAxis;
        [SerializeField] float _lastYAxis;
        [SerializeField] GameObject _highScore;
        [SerializeField] TextMeshProUGUI _currentScoreText;
        [SerializeField] TextMeshProUGUI _highScoreText;
        [SerializeField] Slider _slider;


        [Header("Player")]
        [SerializeField] GameObject _player;
        [SerializeField] GameObject _model;
        [SerializeField] Vector3 _playerFirstPoint;
        [SerializeField] Vector3 _playerScoreFirstPoint;
        [SerializeField] UIManager _uIManager;

        StairsController _stairsController;
        PlayerController _playerController;

        public float CurrentYAxis { get => _currentYAxis; set => _currentYAxis = value; }
        public float LastYAxis { get => _lastYAxis; set => _lastYAxis = value; }

        private void Awake()
        {
            _stairsController = GetComponentInChildren<StairsController>();
            _playerController = GetComponent<PlayerController>();
        }
        private void Start()
        {
            PlayerPrefs.DeleteAll();
            _playerFirstPoint = _model.transform.localPosition;
            _playerScoreFirstPoint = _player.transform.position;
            for (int i = 0; i < _stairsController.StairsList.Count; i++)
            {
                _stairsController.StairsList[i].GetComponent<Rigidbody>().useGravity = false;
                _stairsController.StairsList[i].GetComponent<Rigidbody>().isKinematic = true;
                _stairsController.StairsList[i].GetComponent<Rigidbody>().freezeRotation = true;
            }
        }

        public void Update()
        {
            SetLevelSlider();
            IncreaseScore();
            if (CurrentYAxis == 50)
            {
                GameManager.Instance.IntiliazeGameWon();
            }
        }
        void SetLevelSlider()
        {
            _slider.value = _player.transform.position.y;
        }
        public void IncreaseScore() // Saving The Score and Sing Y Axis
        {
            CurrentYAxis = Mathf.Floor(_player.transform.position.y * 5f);
            LastYAxis = _player.transform.position.y;

            PlayerPrefs.SetFloat("HighScore", CurrentYAxis);
            PlayerPrefs.SetFloat("HighScoreYAxis", LastYAxis);

            _currentScoreText.text = $"{CurrentYAxis}";
        }
        public void ContinueButton()
        {
            StartCoroutine(RestartGame());
        }
        IEnumerator RestartGame()
        {
            for (int i = 0; i < _stairsController.StairsList.Count; i++)
            {
                _stairsController.StairsList[i].GetComponent<Rigidbody>().isKinematic = false;
                _stairsController.StairsList[i].GetComponent<Rigidbody>().useGravity = true;
                _stairsController.StairsList[i].GetComponent<Rigidbody>().freezeRotation = false;
            }

            yield return new WaitForSeconds(1);

            // Getting the Score Value And High Score Sing Y Axis
            _highScoreText.text = $"{PlayerPrefs.GetFloat("HighScore")}";
            _highScore.transform.position = new Vector3(_highScore.transform.position.x, PlayerPrefs.GetFloat("HighScoreYAxis"), _highScore.transform.position.z);

            _model.transform.localPosition = _playerFirstPoint; //Reset Objects Transforms
            _player.transform.position = _playerScoreFirstPoint;
            _player.transform.rotation = Quaternion.identity;

            GameManager.Instance.IntiliazeReadyRoRun();

            for (int i = 0; i < _stairsController.StairsList.Count; i++)
            {
                _stairsController.StairsList[i].SetActive(false);
                _stairsController.StairsList[i].GetComponent<Rigidbody>().useGravity = false;
                _stairsController.StairsList[i].GetComponent<Rigidbody>().isKinematic = true;
                _stairsController.StairsList[i].GetComponent<Rigidbody>().freezeRotation = true;
            }
            GameManager.Instance.Stamina = GameManager.Instance.StaminaStartValue;
            _playerController._sweatingEffect.SetActive(false);
            _uIManager._repeatable = false;
        }
    }
}

