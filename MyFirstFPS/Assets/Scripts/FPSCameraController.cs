using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraController : MonoBehaviour {
    [Header("Camera Settings")]
    public float verticalMouseSensitivity = 0.5f;
    public float horizontalMouseSensitivity = 0.5f;
    public float maximumVerticalRotation = 80f;

    float _xMouseInput, _yMouseInput;
    bool _controlEnabled;

    // Start is called before the first frame update
    void Start() {
        EnableCameraControl();
    }

    // Update is called once per frame
    void Update() {
        if (_controlEnabled) {
            // rotate the camera
            _xMouseInput = Input.GetAxis("Mouse X") * horizontalMouseSensitivity;
            _yMouseInput = Input.GetAxis("Mouse Y") * verticalMouseSensitivity;
            transform.Rotate(transform.parent.up, _xMouseInput, Space.World);
            transform.Rotate(-transform.right, _yMouseInput, Space.World);

            // lock the camera
            float verticalRotation = transform.localEulerAngles.x > 90f ? transform.localEulerAngles.x - 360f : transform.localEulerAngles.x;
            if (verticalRotation > maximumVerticalRotation) {
                transform.localRotation = Quaternion.Euler(maximumVerticalRotation, transform.localEulerAngles.y, 0);
            } else if (verticalRotation < -maximumVerticalRotation) {
                transform.localRotation = Quaternion.Euler(-maximumVerticalRotation, transform.localEulerAngles.y, 0);
            }
        }
    }

    public void EnableCameraControl() {
        _controlEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableCameraControl() {
        _controlEnabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
