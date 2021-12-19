using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    Transform _cameraTransform;
    float _horizontalTrg, _verticalTrg, _currentHorizontalVal, _currentVerticalVal, _horizontalCount = 0, _verticalCount = 0;
    const float _movementSensitivity = 0.1f, _movementSpeed = 0.15f, _horizontalRotationModifier = 5f, _verticalRotationModifier = 7f;
    

    void Start()
    {
        _cameraTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _cameraTransform.rotation;
        transform.Rotate(Vector3.up * -90f);

        _horizontalTrg = Input.GetAxis("Mouse X");
        _verticalTrg = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(_currentHorizontalVal - _horizontalTrg) > _movementSensitivity)
        {
            _currentHorizontalVal = Vector2.Lerp(Vector2.right * _currentHorizontalVal, Vector2.right * _horizontalTrg, _horizontalCount).x;
            _horizontalCount += _movementSpeed * Time.deltaTime;
        }
        else
        {
            _horizontalCount = 0;
        }

        if (Mathf.Abs(_currentVerticalVal - _verticalTrg) > _movementSensitivity)
        {
            _currentVerticalVal = Vector2.Lerp(Vector2.right * _currentVerticalVal, Vector2.right * _verticalTrg, _verticalCount).x;
            _verticalCount += _movementSpeed * Time.deltaTime;
        }
        else
        {
            _verticalCount = 0;
        }


        transform.Rotate(transform.up, _currentHorizontalVal * _horizontalRotationModifier, Space.World);
        transform.Rotate(-transform.forward, _currentVerticalVal * _verticalRotationModifier, Space.World);
    }
}
