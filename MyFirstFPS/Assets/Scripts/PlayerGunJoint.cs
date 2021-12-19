using System.Collections;
using UnityEngine;

public class PlayerGunJoint : MonoBehaviour {
    // This script deals with the movement of the gun
    // Rotation and translation

    [SerializeField]
    Transform cameraTransform;

    float _horizontalTarget, _verticalTarget, _horizontalVal, _verticalVal, _horizontalCount = 0, _verticalCount = 0;
    const float _movementSensitivity = 0.1f, _movementSpeed = 0.15f, _horizontalRotationModifier = 2f, _verticalRotationModifier = 1.75f;

    // Update is called once per frame
    void Update() {
        transform.rotation = cameraTransform.rotation;
        _horizontalTarget = Input.GetAxis("Mouse X");
        _verticalTarget = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(_horizontalVal - _horizontalTarget) > _movementSensitivity) {
            _horizontalVal = Vector2.Lerp(Vector2.right * _horizontalVal, Vector2.right * _horizontalTarget, _horizontalCount).x;
            _horizontalCount += _movementSpeed * Time.deltaTime;
        } else {
            _horizontalCount = 0;
        }

        if (Mathf.Abs(_verticalVal - _verticalTarget) > _movementSensitivity) {
            _verticalVal = Vector2.Lerp(Vector2.right * _verticalVal, Vector2.right * _verticalTarget, _verticalCount).x;
            _verticalCount += _movementSpeed * Time.deltaTime;
        } else {
            _verticalCount = 0;
        }

        transform.Rotate(transform.up, _horizontalVal * _horizontalRotationModifier, Space.World);
        transform.Rotate(-transform.right, _verticalVal * _verticalRotationModifier, Space.World);
    }
}
