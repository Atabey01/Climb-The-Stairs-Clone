using ClimbTheStairs.Concretes;
using ClimbTheStairs.Managers;
using ClimbTheStairs.Stairs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimbTheStairs.Player
{
    public class PlayerController : MonoBehaviour
    {

        [Header("Gameplay")]
        [SerializeField] GameObject _scoreWood;
        [SerializeField] GameObject _wonPosition;
        [SerializeField] GameObject _explosionEffect;
        public ParticleSystem _hitbuttonEffect;
        public GameObject _sweatingEffect;
        [SerializeField] GameObject _model;
        [SerializeField] AudioSource _bubbleSound;

        [Header("Scripts")]
        [SerializeField] UIManager _uIManager;
        [SerializeField] PointsController _pointsController;
        [SerializeField] StairsController _stairsController;

        Mover _mover;
        public Animator _animator;
        float _count;

        //public float _stamina;
        public bool _repeatable;



        private void Awake()
        {
            _mover = new Mover(this);
            _animator = GetComponentInChildren<Animator>();
        }
        private void Start()
        {
            _count = 0.4f;
            GameManager.Instance.StaminaStartValue = 15;
            GameManager.Instance.Stamina = GameManager.Instance.StaminaStartValue;
            GameManager.Instance.GoldIncreaseAmount = 0.5f;
            _explosionEffect.SetActive(false);
            _sweatingEffect.SetActive(false);
        }
        private void OnEnable()
        {
            GameManager.Instance.OnReadyToRun += SetStartColor;
            GameManager.Instance.OnReadyToRun += SetPlayerAnim;

            GameManager.Instance.OnGameStarted += SetGameStarted;

            GameManager.Instance.OnGameWon += SetPlayerPositionOnWon;
        }

        private void SetGameStarted()
        {
            _animator.SetBool("Won", false);

        }

        private void SetPlayerAnim()
        {
            _animator.SetBool("Won", false);
        }

        private void SetPlayerPositionOnWon()
        {
            _sweatingEffect.SetActive(false);
            _animator.SetBool("isRun", false);
            _animator.SetBool("Won", true);
            _uIManager._repeatable = false;
            _stairsController._modelMat.color = Color.Lerp(_stairsController._modelMat.color, _stairsController._whiteColor, 0.02f);

        }

        private void SetStartColor()
        {
            _stairsController._modelMat.color = _stairsController._whiteColor;
        }

        private void FixedUpdate()
        {
            _scoreWood.transform.position = new Vector3(transform.position.x, transform.position.y + 0.22f, transform.position.z);
            if (GameManager.Instance.GameState == Abstracts.GameStates.InGameStarted)
            {
                if (Input.GetMouseButton(0)) // Tap Move
                {
                    if (GameManager.Instance.Stamina > 0)
                    {
                        GameManager.Instance.Stamina -= Time.deltaTime;
                        _count += Time.deltaTime;
                        _animator.SetBool("isRun", true);
                        _mover.TickFixed();
                        if (_count > 0.2f)
                        {
                            #region Stairs Spawn
                            _bubbleSound.Play();
                            _stairsController.StairsRoutine();
                            StartCoroutine(_pointsController.PointsRoutine());
                            float lerpY = Mathf.Lerp(transform.position.y, transform.position.y + 0.05f, 3f);
                            transform.position = new Vector3(transform.position.x, lerpY, transform.position.z);

                            _count = 0;
                            #endregion
                        }
                    }
                }
                else
                {
                    _animator.SetBool("isRun", false);

                }
                if (GameManager.Instance.Stamina <= 0)
                {
                    GameManager.Instance.IntiliazeGameLost();
                    _explosionEffect.SetActive(true);
                    _animator.SetBool("isRun", false);

                }
                if (GameManager.Instance.Stamina < 5)
                {
                    GameManager.Instance.IntiliazeTimeRunningOut();
                    _stairsController._modelMat.color = Color.Lerp(_stairsController._modelMat.color, _stairsController._redColor, 0.02f);

                    _uIManager._repeatable = true;
                    _sweatingEffect.SetActive(true);
                }
                if (GameManager.Instance.Stamina >= 5)
                {
                    _uIManager._repeatable = false;
                    _sweatingEffect.SetActive(false);
                    _stairsController._modelMat.color = Color.Lerp(_stairsController._modelMat.color, _stairsController._whiteColor, 0.02f);
                }

                GameManager.Instance.Stamina += 0.2f * Time.deltaTime;
            }
        }
    }
}
