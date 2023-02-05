using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    const float _mouseSpeed = 1f;
    float _mouseY;
    Vector3 _initPos;

    private void Start()
    {
        _initPos = transform.localPosition;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        _mouseY -= Input.GetAxis("Mouse Y");
        _mouseY = Math.Min(Math.Max(-90, _mouseY), 90);
        transform.localPosition = _initPos + new Vector3(0, 0, Math.Abs(_mouseY * .06f));
        transform.localRotation = Quaternion.Euler(_mouseY * _mouseSpeed, 0, 0);
    }
}
