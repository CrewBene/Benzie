using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    const float _moveSpeed = 10f;
    const float _jumpSpeed = 20f;
    const float _danceSpeed = 300f;
    const float _mouseSpeed = 1f;
    LayerMask _groundLayer;
    Rigidbody _playerRB;
    Transform _playerLeftArm;
    Transform _playerRightArm;
    int _danceMode = 0;
    float _danceAngle = 60;
    float _mouseX;

    void Start()
    {
        _playerRB = transform.GetComponent<Rigidbody>();
        _playerLeftArm = transform.Find("PlayerBody/PlayerLeftArm");
        _playerRightArm = transform.Find("PlayerBody/PlayerRightArm");
        _groundLayer = LayerMask.GetMask(new[] { "GroundCheck" });
    }
    
    void Update()
    {
        // Gravity
        _playerRB.AddForce(0, -2000 * Time.deltaTime, 0, ForceMode.Acceleration);

        // Direction
        _mouseX += Input.GetAxis("Mouse X");
        transform.localRotation = Quaternion.Euler(0, _mouseX * _mouseSpeed, 0);
        
        // Move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 flatVelocity = Quaternion.AngleAxis(transform.localRotation.eulerAngles.y, Vector3.up) * new Vector3(horizontalInput * _moveSpeed, 0, verticalInput * _moveSpeed);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerRB.velocity = new Vector3(flatVelocity.x * 2, _playerRB.velocity.y, flatVelocity.z * 2);
        }
        else
        {
            _playerRB.velocity = new Vector3(flatVelocity.x, _playerRB.velocity.y, flatVelocity.z);
        }

        // Jump
        Jump();

        // Dance
        Dance();

        // Fall of map
        if (_playerRB.position.y < -10)
        {
            SceneManager.LoadScene("Castle", LoadSceneMode.Single);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool onGround = Physics.CheckSphere(_playerRB.position, .3f, _groundLayer);
            if (onGround)
            {
                _playerRB.velocity = new Vector3(_playerRB.velocity.x, _jumpSpeed, _playerRB.velocity.z);
            }
        }

    }

    void Dance()
    {
        if (Input.GetKeyDown(KeyCode.F) && _danceMode == 0)
        {
            _danceMode = 1;
        }

        if (_danceMode > 0)
        {
            if ((_danceMode & 1) == 1)
            {
                _danceAngle = Math.Max(-60, _danceAngle - _danceSpeed * Time.deltaTime);
            }
            else
            {
                _danceAngle = Math.Min(60, _danceAngle + _danceSpeed * Time.deltaTime);
            }

            if (Math.Abs(_danceAngle) >= 60)
            {
                _danceMode++;
                if (_danceMode >= 7)
                {
                    _danceMode = 0;
                    _danceAngle = 60;
                }
            }

            _playerLeftArm.localRotation = Quaternion.Euler(new Vector3(0, 0, -_danceAngle));
            _playerRightArm.localRotation = Quaternion.Euler(new Vector3(0, 0, _danceAngle));
        }
    }
}
