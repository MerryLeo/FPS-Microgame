using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject playerObj;

    [Header("Mouse Settings")]
    public float verticalSensitivity = 0.6f, horizontalSensitivity = 0.6f, maximumVerticalRotation = 80f;
    float _xMouseInput, _yMouseInput;
    bool _controlOn;

    // Start is called before the first frame update
    void Start() {
        ActivateControl();
    }

    // Update is called once per frame
    void Update() {
        if (_controlOn) {
            _xMouseInput = Input.GetAxis("Mouse X") * horizontalSensitivity;
            _yMouseInput = Input.GetAxis("Mouse Y") * verticalSensitivity;
            transform.Rotate(playerObj.transform.up, _xMouseInput, Space.World);
            transform.Rotate(-transform.right, _yMouseInput, Space.World);

            float verticalRotation = transform.localEulerAngles.x > 90f ? transform.localEulerAngles.x - 360f : transform.localEulerAngles.x;
            if (verticalRotation > maximumVerticalRotation) {
                transform.localRotation = Quaternion.Euler(maximumVerticalRotation, transform.localEulerAngles.y, 0);
            } else if (verticalRotation < -maximumVerticalRotation) {
                transform.localRotation = Quaternion.Euler(-maximumVerticalRotation, transform.localEulerAngles.y, 0);
            }
        }
    }

    public void DisableControl() {
        _controlOn = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ActivateControl() {
        _controlOn = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
