using ClimbTheStairs.Concretes;
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using ClimbTheStairs.Player;
using System;
using ClimbTheStairs.Stairs;

namespace ClimbTheStairs.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] GameObject _startPanel;
        [SerializeField] GameObject _gamePanel;
        [SerializeField] GameObject _restartPanel;
        [SerializeField] GameObject _wonPanel;

        [Header("Score")]
        [SerializeField] GameObject _highScore;
        [SerializeField] TextMeshProUGUI _highScoreText;

        [Header("Player")]
        [SerializeField] GameObject _model;
        [SerializeField] GameObject _modelSpine;
        [SerializeField] Vector3 _modelMaxScale;
        [SerializeField] Vector3 _modelMinScale;
        [SerializeField] float _modelScaleSpeed;
        [SerializeField] float _modelScaleDuration;

        [Header("Scripts")]
        [SerializeField] PointsController _pointController;
        [SerializeField] PlayerController _playerController;


        [Header("Shop")]
        [SerializeField] TextMeshProUGUI _incomeText;
        [SerializeField] TextMeshProUGUI _staminaText;
        [SerializeField] float _incomeUpValue;
        [SerializeField] float _staminaUpValue;


        public bool _repeatable;

        SaveScore _saveScore;
        private void OnEnable()
        {
            GameManager.Instance.OnReadyToRun += SetPanelsOnReadyToRun;

            GameManager.Instance.OnGameStarted += SetPanelsOnGameStarted;

            GameManager.Instance.OnGameLost += SetPanelsOnGameLost;

            GameManager.Instance.OnGameWon += SetPanelOnGameWon;

            GameManager.Instance.OnTimeRunningOut += SetTimeRanningOut;

        }

        private void SetPanelOnGameWon()
        {
            _wonPanel.SetActive(true);
        }

        private void SetTimeRanningOut()
        {
            StartCoroutine(SetPlayerScale());
        }

        private void SetPanelsOnGameLost()
        {
            _startPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _restartPanel.SetActive(true);
            _model.SetActive(false);
        }

        private void SetPanelsOnGameStarted()
        {
            _startPanel.SetActive(false);
            _gamePanel.SetActive(true);
            _restartPanel.SetActive(false);
        }

        private void SetPanelsOnReadyToRun()
        {
            _startPanel.SetActive(true);
            _gamePanel.SetActive(true);
            _restartPanel.SetActive(false);
            _model.SetActive(true);
            _wonPanel.SetActive(false);

        }
        public void TapToStartButton()
        {
            GameManager.Instance.IntiliazeGameStarted();
        }
        public void TapToStaminaButton()
        {
            _staminaUpValue = Convert.ToInt32(_staminaText.text);
            if (_staminaUpValue < GameManager.Instance.Gold)
            {
                print("Paran Yeterli");
                _playerController._hitbuttonEffect.Play();
                _playerController._animator.SetBool("Won", true);
                GameManager.Instance.Stamina = GameManager.Instance.StaminaIncreaseAmount + GameManager.Instance.StaminaStartValue;
                GameManager.Instance.StaminaStartValue = GameManager.Instance.Stamina;
                GameManager.Instance.Gold = GameManager.Instance.Gold - _staminaUpValue;
            }
            else
            {
                print("paran yetersiz");
            }
        }
        public void TapToIncomeButton()
        {
            _incomeUpValue = Convert.ToInt32(_incomeText.text);
            print(_incomeUpValue);
            if (_incomeUpValue < GameManager.Instance.Gold)
            {
                print("Paran Yeterli");
                _playerController._hitbuttonEffect.Play();
                _playerController._animator.SetBool("Won", true);
                GameManager.Instance.GoldIncreaseAmount = GameManager.Instance.BuyGoldIncreaseAmount + GameManager.Instance.GoldIncreaseAmount;
                GameManager.Instance.Gold = GameManager.Instance.Gold - _incomeUpValue;
            }
            else
            {
                print("paran yetersiz");
            }
        }

        IEnumerator SetPlayerScale() // To Scale Up And Down on Time Is Running Out
        {
            while (_repeatable)
            {
                yield return RepeatLerp(_modelMinScale, _modelMaxScale, _modelScaleDuration);
                yield return RepeatLerp(_modelMaxScale, _modelMinScale, _modelScaleDuration);
            }
        }
        public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
        {
            float i = 0;
            float rate =(1 / time) * _modelScaleSpeed;
            while (i < 1)
            {
                i += Time.deltaTime * rate;
                _modelSpine.transform.localScale = Vector3.Lerp(a, b, i);
                yield return null;  
            }

        }
    }
}
