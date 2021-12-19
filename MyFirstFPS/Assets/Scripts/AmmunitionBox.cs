using System.Collections;
using UnityEngine;

public class AmmunitionBox : MonoBehaviour 
{
    [SerializeField] [Range(0, 100)]
    int ammo = 25;

    PickableObjectBehaviour _behaviourScript;
    MeshRenderer _meshRenderer;
    const float _respawnCooldown = 15f;
    bool _isActive = true;

    void Start() 
    {
        _behaviourScript = GetComponent<PickableObjectBehaviour>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other) 
    {
        Trigger(other);
    }

    void OnTriggerStay(Collider other) 
    {
        Trigger(other);
    }

    void Trigger(Collider other) 
    {
        if (other.tag == "Player" && _isActive) 
        {
            PlayerController_FSM playerControllerScript = other.GetComponent<PlayerController_FSM>();
            playerControllerScript.currentGun.AddAmmo(ammo);
            StartCoroutine(DisableAmmoBox());
        }
    }

    IEnumerator DisableAmmoBox() 
    {
        _isActive = false;
        _behaviourScript.SetActive(false);
        _meshRenderer.enabled = false;
        yield return new WaitForSeconds(_respawnCooldown);
        _meshRenderer.enabled = true;
        _behaviourScript.SetActive(true);
        _isActive = true;
    }
}
