using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] NDirection _direction = NDirection.X;
    [SerializeField] float _distance = 1f;
    [SerializeField] float _moveSpeed = 1f;
    bool _moveBack = false;
    float _offset = 0;
    Vector3 _initPos;

    enum NDirection
    {
        X,
        Y,
        Z,
    }

    void Start()
    {
        _initPos = transform.position;
    }

    void Update()
    {
        // Change direction
        if (_distance > 0 == _offset >= _distance)
        {
            _moveBack = true;
        }
        else if (_distance > 0 == _offset <= 0)
        {
            _moveBack = false;
        }

        // Calculate position
        if (_distance > 0 == !_moveBack)
        {
            _offset += Time.deltaTime * _moveSpeed;
        }
        else
        {
            _offset -= Time.deltaTime * _moveSpeed;
        }

        // Set position
        switch (_direction)
        {
            case NDirection.X:
                transform.position = new Vector3(_initPos.x + _offset, _initPos.y, _initPos.z);
                break;
            case NDirection.Y:
                transform.position = new Vector3(_initPos.x, _initPos.y + _offset, _initPos.z);
                break;
            case NDirection.Z:
                transform.position = new Vector3(_initPos.x, _initPos.y, _initPos.z + _offset);
                break;
        }
    }
}
