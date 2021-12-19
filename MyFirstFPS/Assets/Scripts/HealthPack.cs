using System.Collections;
using UnityEngine;

public class HealthPack : MonoBehaviour {
    [SerializeField]
    int health = 25;

    PickableObjectBehaviour _behaviourScript;
    MeshRenderer _meshRenderer;
    const float _respawnCooldown = 12f;
    bool _isActive = true;

    // Start is called before the first frame update
    void Start() {
        _behaviourScript = GetComponent<PickableObjectBehaviour>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other) {
        Trigger(other);
    }

    void OnTriggerStay(Collider other) {
        Trigger(other);
    }

    void Trigger(Collider other) {
        if (other.tag == "Player" && _isActive) {
            PlayerStatus statScript = other.GetComponent<PlayerStatus>();
            if (statScript.Health < statScript.MaxHealth) {
                statScript.AddHealth(health);
                StartCoroutine(DisableHealthPack());
            }
        }
    }

    IEnumerator DisableHealthPack() {
        _isActive = false;
        _behaviourScript.SetActive(false);
        _meshRenderer.enabled = false;
        yield return new WaitForSeconds(_respawnCooldown);
        _meshRenderer.enabled = true;
        _behaviourScript.SetActive(false);
        _isActive = true;
    }
}
