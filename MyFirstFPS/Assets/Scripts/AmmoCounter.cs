using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour, IPlayerUIElement {
    [SerializeField] 
    Image middlebarImg;
    [SerializeField] 
    Text ammunitionOutOfClip;
    [SerializeField] 
    Text ammunitionInClipCounterText;
    [SerializeField] 
    Text reloadingText;

    bool _beginCalled = false;
    IPlayerFirearm _playerCurrentGun;

    // Update is called once per frame
    void Update() {
        if (_beginCalled) {
            if (_playerCurrentGun.Reloading) {
                ShowReloadingText();
            } else {
                ShowAmmunition();
            }
        }
    }

    void ShowReloadingText() {
        if (!reloadingText.IsActive()) {
            reloadingText.gameObject.SetActive(true);
        }

        if (ammunitionOutOfClip.IsActive() || ammunitionInClipCounterText.IsActive() || middlebarImg.IsActive()) {
            ammunitionOutOfClip.gameObject.SetActive(false);
            ammunitionInClipCounterText.gameObject.SetActive(false);
            middlebarImg.gameObject.SetActive(false);
        }
    }

    void ShowAmmunition() {
        if (reloadingText.IsActive()) {
            reloadingText.gameObject.SetActive(false);
        }

        if (!ammunitionOutOfClip.IsActive() || !ammunitionInClipCounterText.IsActive() || !middlebarImg.IsActive()) {
            ammunitionOutOfClip.gameObject.SetActive(true);
            ammunitionInClipCounterText.gameObject.SetActive(true);
            middlebarImg.gameObject.SetActive(true);
        }

        ammunitionInClipCounterText.text = _playerCurrentGun.AmmunitionInClip.ToString();
        ammunitionOutOfClip.text = _playerCurrentGun.AmmunitionOutOfClip.ToString();
    }

    public void Begin(GameObject playerObj) {
        _playerCurrentGun = playerObj.GetComponent<PlayerController_FSM>().currentGun;
        _beginCalled = true;
    }
}
