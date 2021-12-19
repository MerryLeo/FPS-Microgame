using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {
    
    [SerializeField]
    GameObject enemyHealthbarPrefab;
    [SerializeField] [InspectorName("Mesh To Blink")]
    MeshRenderer[] meshRenderers;

    EnemyHealthbar _healthbarScript;
    Enemy _enemyScript;

    public int MaxHealth { get; private set; } = 100;
    public int Health { get; private set; } = 100;

    const float _blinkDuration = 0.45f;

    MeshRenderer meshRenderer;
    Color _objectColor;
    Color _takeDamageColor = Color.red;
    bool _finishedBlinking = true;

    void Awake() {
        _enemyScript = GetComponent<Enemy>();
        Health = MaxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        _objectColor = meshRenderer.material.color;

        _healthbarScript = Instantiate(enemyHealthbarPrefab).GetComponent<EnemyHealthbar>();
        _healthbarScript.Begin(GameObject.Find("Enemy Canvas").transform, this);
    }

    void Update() {
        _healthbarScript.UpdatePosition(transform.position);
    }

    public void TakeDamage(int dmg) {
        Health -= dmg;
        if (!_enemyScript.IsAwake) {
            _enemyScript.WakeUp();
        }

        if (Health <= 0) {
            Die();
        } else if (_finishedBlinking && meshRenderers.Length > 0) {
            StartCoroutine(Blink());
        }
    }

    void Die() {
        gameObject.SetActive(false);
    }

    IEnumerator Blink() {
        _finishedBlinking = false;
        for (int i = 0; i < meshRenderers.Length; i++) {
            meshRenderers[i].material.color = _takeDamageColor;
        }
        yield return new WaitForSeconds(_blinkDuration);
        for (int i = 0; i < meshRenderers.Length; i++) {
            meshRenderers[i].material.color = _objectColor;
        }
        _finishedBlinking = true;
    }
}
