using ClimbTheStairs.Abstracts;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbTheStairs.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Events
        public event Action OnReadyToRun;
        public event Action OnGameStarted;
        public event Action OnTimeRunningOut;
        public event Action OnGameLost;
        public event Action OnGameWon;
        public event Action OnGameStopped;
        #endregion

        #region Singleton
        [Header("Shop")]
        [SerializeField] private float _gold;
        [SerializeField] private float _stamina;
        [SerializeField] private float _staminaStartValue;
        [SerializeField] private float _goldIncreaseAmount;
        [SerializeField] private float _buyGoldIncreaseAmount;
        [SerializeField] private float _staminaIncreaseAmount;
        public static GameManager Instance { get; private set; }
        public GameStates GameState { get; private set; }
        public float Gold { get => _gold; set => _gold = value; }
        public float GoldIncreaseAmount { get => _goldIncreaseAmount; set => _goldIncreaseAmount = value; }
        public float Stamina { get => _stamina; set => _stamina = value; }
        public float StaminaIncreaseAmount { get => _staminaIncreaseAmount; set => _staminaIncreaseAmount = value; }
        public float BuyGoldIncreaseAmount { get => _buyGoldIncreaseAmount; set => _buyGoldIncreaseAmount = value; }
        public float StaminaStartValue { get => _staminaStartValue; set => _staminaStartValue = value; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion
        private void Start()
        {
            IntiliazeReadyRoRun();
        }
        private void Update()
        {
            print(GameState);
        }
        public void IntiliazeReadyRoRun()
        {
            GameState = GameStates.InReadyToRun;
            OnReadyToRun?.Invoke();
        }
        public void IntiliazeGameStarted()
        {
            GameState = GameStates.InGameStarted;
            OnGameStarted?.Invoke();
        }
        public void IntiliazeTimeRunningOut()
        {          
            OnTimeRunningOut?.Invoke();
        }
        public void IntiliazeGameLost()
        {
            GameState = GameStates.InGameLost;
            OnGameLost?.Invoke();
        }
        public void IntiliazeGameWon()
        {
            GameState = GameStates.InGameWon;
            OnGameWon?.Invoke();
        }
        public void IntiliazeGameStopped()
        {
            GameState = GameStates.InGameStopped;
            OnGameStopped?.Invoke();
        }
    } 
}
