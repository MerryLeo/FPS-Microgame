using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour, IPlayerUIElement {
    bool _beginCalled = false;
    GameObject _playerCameraViewObj;
    [SerializeField]
    GameObject _neutralSight;
    [SerializeField]
    GameObject _negativeSight;

    void Update() {
        if (_beginCalled) {
            RaycastHit hit;
            Ray ray = new Ray(_playerCameraViewObj.transform.position, _playerCameraViewObj.transform.forward);
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Enemy") {
                _neutralSight.SetActive(false);
                _negativeSight.SetActive(true);
            } else if (_neutralSight.activeInHierarchy == false) {
                _neutralSight.SetActive(true);
                _negativeSight.SetActive(false);
            }
        }
    }

    public void Begin(GameObject playerObj) {
        _playerCameraViewObj = playerObj.GetComponentInChildren<FPSCameraController>().gameObject;
        _neutralSight.SetActive(true);
        _negativeSight.SetActive(false);
        _beginCalled = true;
    }
}
