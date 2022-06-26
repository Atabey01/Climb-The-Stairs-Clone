using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Movement : MonoBehaviour
{
    #region GameObject Veriables
    [SerializeField] GameObject _character;
    [SerializeField] GameObject _meterCounterSign;
    [SerializeField] GameObject _highScoreSign;
    [SerializeField] GameObject _highScoreWood;
    [SerializeField] GameObject _ScoreSign;
    [SerializeField] GameObject _stairsPrefab;
    [SerializeField] GameObject _camera;
    [SerializeField] GameObject _menuPanel;
    [SerializeField] GameObject _levelComplatePanel;
    [SerializeField] GameObject _moneyObject;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] Transform _stairsSpawnPoint;
    #endregion

    #region GameObject Lists
    [SerializeField] List<GameObject> _stairsList = new List<GameObject>();
    [SerializeField] List<GameObject> _meterWoodCountList = new List<GameObject>();
    #endregion

    #region Animators
    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorMoney;
    [SerializeField] Animator _animatorLowStamina;
    #endregion
    
    #region Text Veriables
    [SerializeField] Text _scoreText;
    [SerializeField] Text _highScoreText;
    [SerializeField] Text _moneyText;
    [SerializeField] Text _moneyAmountText;
    #endregion
    
    #region Float Veriables
    [SerializeField] float _characterRotationSpeed = 100f;
    [SerializeField] float _score;
    [SerializeField] float _maxScore;
    [SerializeField] float _money;
    [SerializeField] float _moneyAmount;
    [SerializeField] float _stamina = 50;
    [SerializeField] float _meterCounterSignPoint;
    float _rate = 0;
    #endregion
    
    #region Bool Veriables
    bool _isMoving;
    bool _isGameStarted;
    #endregion
   
    [SerializeField] ParticleSystem _gameOverParticalEffect;
    
    [SerializeField] Material _material;

    [SerializeField] Slider _levelSlider;
 
    Rigidbody _stairsRigigBody;
    
    Color _beginingColor;

    private void Start()
    {
        //Restart
        _menuPanel.SetActive(true);
        _gameOverPanel.SetActive(false);
        _levelComplatePanel.SetActive(false);

        _isGameStarted = false;

        High_Score();

        //Movement According By Stamina
        _stamina = 50;

        _stairsRigigBody = _stairsPrefab.GetComponent<Rigidbody>();
        _stairsRigigBody.useGravity = false;
        _animatorLowStamina.enabled = false;

        _isMoving = true;


        // Low Stamina Color Change
        _material.color = new Color(0.7490196f, 0.7490196f, 0.7529412f, 1);
        _beginingColor = _material.color;

        _camera.transform.position = new Vector3(0, (_character.transform.position.y + 1.8f), -4);
    }
    void FixedUpdate()
    {
        if (_isGameStarted == true)
        {
            Cam_Offset();
            Character_Movement();
        }
    }

    //This Method Is Working When _isGameStared Bool Is True
    void Character_Movement()
    {
        //Stamina Increase
        if (_stamina <= 50)
        {
            _stamina += 1.5f * Time.fixedDeltaTime;
        }

        // Animation and Color Change When Stamina Grater Than 15
        if (_stamina > 15)
        {
            _animatorLowStamina.enabled = false;
            _material.color = _beginingColor;

        }

        //When We Touch The Screen
        if (Input.GetMouseButton(0))
        {

            if (_isMoving == true)
            {
                Move();
                _rate += Time.deltaTime * 7;
                _stamina -= 5 * Time.fixedDeltaTime;
            }

            //Distance Between Stairs
            if (_rate > 1 && _stamina >= 0 && _isMoving == true)
            {
                Game_Play();

                _rate = 0;

                _meterCounterSignPoint += 0.02f;

                if (_stamina > 0 && _stamina <= 15)
                {
                    _animatorLowStamina.enabled = true;
                    _material.color = Color.Lerp(Color.red, _beginingColor, 0.01f * Time.deltaTime);

                }
            }

            //Game Over
            if (_stamina <= 0)
            {
                _isMoving = false;
                _isGameStarted = false;

                _animatorLowStamina.enabled = false;
                _animator.SetBool("Move", false);

                Destroy(_character, 0.3f);
                _gameOverParticalEffect.Play();

                PlayerPrefs.SetFloat("HighScoreSignY", _ScoreSign.transform.position.y);

                _gameOverPanel.SetActive(true);


                //Stair Motion When Game Over
                foreach (var stairs in _stairsList)
                {
                    stairs.GetComponent<Rigidbody>().useGravity = true;
                    stairs.GetComponent<Rigidbody>().AddForce(Vector3.up * 400f * Time.deltaTime);
                    stairs.GetComponent<Animator>().SetTrigger("GameOverStairsAnim");

                    Destroy(stairs, 3f);
                }

                //StartCoroutine(Game_Over());

            }

        }

        else
        {
            _animator.SetBool("Move", false);
        }
    }

    //Game_Play Is Working In The Character_Movement
    void Game_Play()
    {
        //Score
        _score += 0.25f;
        PlayerPrefs.SetFloat("Score", _score);
        _scoreText.text = "" + _score + "m";

        //Level Complate
        if (_score == 50)
        {
            _isMoving = false;
            _isGameStarted = false;
            _levelComplatePanel.SetActive(true);
            _animatorLowStamina.enabled = false;
            _animator.SetBool("Move", false);

            PlayerPrefs.DeleteAll();
        }

        //Money
        _money = 0.5f;
        _moneyText.text = "$" + _money;

        _moneyAmount += 0.5f;
        _moneyAmountText.text = "" + _moneyAmount;

        // Level Slider
        _levelSlider.value = _score;

        //Stairs Instantiate
        GameObject StairsClone = Instantiate(_stairsPrefab, _stairsSpawnPoint.transform.position, Quaternion.LookRotation(new Vector3(_stairsSpawnPoint.position.x, 0, _stairsSpawnPoint.position.z)));
        _stairsList.Add(StairsClone);

        if (_stairsList.Count > 31)
        {
            Destroy(_stairsList[0]);
            _stairsList.RemoveAt(0);
        }

        //Meter Counter Wood Instantiate
        GameObject MeterWood = Instantiate(_meterCounterSign, new Vector3(0, _ScoreSign.transform.position.y - 0.25f, 0), Quaternion.identity);
        _meterWoodCountList.Add(MeterWood);

        if (_meterWoodCountList.Count > 31)
        {
            Destroy(_meterWoodCountList[0]);
            _meterWoodCountList.RemoveAt(0);
        }

        //Money Text Effect Instantiate
        GameObject Money = Instantiate(_moneyObject, new Vector3(_stairsSpawnPoint.transform.position.x + 0.2f, _stairsSpawnPoint.transform.position.y + 0.2f, _stairsSpawnPoint.transform.position.z - 0.5f), Quaternion.identity);
        _animatorMoney.SetTrigger("MoneyAnim");

        Destroy(Money, 0.45f);
    }

    //This Method Is Working When _isGameStared Bool Is True
    void Cam_Offset()
    {
        _camera.transform.position = new Vector3(0, (_character.transform.position.y + 1.8f), -4);
    }

    //Game Move Controls
    void Move()
    {
        //Character Move
        transform.Rotate(new Vector3(0, 1, 0) * _characterRotationSpeed * Time.deltaTime);
        _character.transform.position += new Vector3(0, 0.5f * Time.deltaTime, 0);
        _animator.SetBool("Move", true);

        //Score Sign Move
        _ScoreSign.transform.Rotate(new Vector3(0, -1, 0) * _characterRotationSpeed * Time.deltaTime);
        _ScoreSign.transform.position += new Vector3(0, 0.5f * Time.deltaTime, 0);
    }

    //High Score Save
    void High_Score()
    {
        // High Score
        _maxScore = PlayerPrefs.GetFloat("Score");
        float _signY = PlayerPrefs.GetFloat("HighScoreSignY");

        if (_maxScore > _score)
        {
            _highScoreText.text = "" + _maxScore + "m";

            _highScoreSign.transform.position = new Vector3(1f, _signY, -0.25f);
            print(_signY);
        }
    }

    //Top To Start Button
    public void Game_Start()
    {
        _menuPanel.SetActive(false);
        _isGameStarted = true;
    }


    public void  Restart_Game()
    {
        StartCoroutine(Reload_Scene());
    }

    //Reload Active Scene
    IEnumerator Reload_Scene()
    {
        _gameOverPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);
  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }  
    public void Exit_Game()
    {
        Application.Quit(); 
    }
}
