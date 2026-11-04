using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 3f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    //[SerializeField]
    private bool _tripleShotEnabled = false;
    private bool _SpeedMultEnalbed = false;

    private bool _ShieldEnabled = false;
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private int _score;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.LogError("the spawn manager is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);
        if (_SpeedMultEnalbed == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }
    }

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_tripleShotEnabled == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_ShieldEnabled == true)
        {
            _ShieldEnabled = false;
            _shield.SetActive(false);
            return;
        }
        else { 
            _lives--;
        }
        if(_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    IEnumerator resetPowerUp()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotEnabled = false;
    }

    IEnumerator resetSpeed()
    {
        yield return new WaitForSeconds(5.0f);
        _SpeedMultEnalbed = false;
    }

    //IEnumerator resetShield()
    //{
    //    yield return new WaitForSeconds(5.0f);
    //    _ShieldEnabled = false;
    //}

    public void SetTripleShot()
    {
        _tripleShotEnabled = true;
        StartCoroutine(resetPowerUp());
    }

    public void SetSpeed()
    {
        _SpeedMultEnalbed = true;
        StartCoroutine(resetSpeed());
    }

    public void SetShield()
    {
        _ShieldEnabled = true;
        _shield.SetActive(true);
        //StartCoroutine(resetShield());
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public int GetScore()
    {
        return _score;
    }
}
