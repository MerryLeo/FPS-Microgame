using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour {
    const float elevation = 1.5f, _maxVisibilityDst = 10f, _minVisibilityDst = 5f, _minSlope = 0.4f, _maxSlope = 0.2f, _speed = 2f;
    int _targetValue;
    float _count, _transperency;
    Slider _slider;
    GameObject _playerObj;
    Image[] _imgs;
    EnemyStatus _enemyStatusScript;

    void Awake() {
        _slider = GetComponent<Slider>();
        _imgs = GetComponentsInChildren<Image>();
        _playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void Start() {
        UpdateTransperency();
    }

    void Update() {
        UpdateTransperency();
        if (_targetValue != _enemyStatusScript.Health) {
            UpdateHealth();
        }

        if (_slider.value != _targetValue) {
            Vector2 position = Vector2.Lerp(new Vector2(_slider.value, 0), new Vector2(_targetValue, 0), _count);
            _slider.value = position.x;
            _count += _speed * Time.deltaTime;
        } else if (_count != 0) {
            _count = 0f;
        }
    }

    void UpdateTransperency() {
        float distance = Vector3.Distance(transform.position, _playerObj.transform.position);
        if (distance > _minVisibilityDst && distance < _maxVisibilityDst) {
            _transperency = 1f;
        } else if (distance <= _minVisibilityDst) {
            _transperency = _minSlope * distance - _minSlope * _minVisibilityDst + 1f;
        } else if (distance >= _maxVisibilityDst) {
            _transperency = -_maxSlope * distance + _maxSlope + _maxVisibilityDst + 1f;
        }
        _transperency = Mathf.Clamp01(_transperency);
        for (int i = 0; i < _imgs.Length; i++) {
            _imgs[i].color = new Color(_imgs[i].color.r, _imgs[i].color.g, _imgs[i].color.b, _transperency);
        }
    }

    public void UpdateHealth() {
        int hp = _enemyStatusScript.Health;
        _count = 0;
        if (hp <= 0) {
            _targetValue = 0;
            gameObject.SetActive(false);
        } else if (hp > _slider.maxValue) {
            _targetValue = _enemyStatusScript.MaxHealth;
        } else {
            _targetValue = hp;
        }
    }

    public void Begin(Transform parent, EnemyStatus enemyStatus) {
        transform.SetParent(parent);
        _enemyStatusScript = enemyStatus;
        _slider.value = enemyStatus.Health;
        _slider.maxValue = enemyStatus.MaxHealth;
    }

    public void UpdatePosition(Vector3 position) {
        transform.position = position + Vector3.up * elevation;
    }
}
