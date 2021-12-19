using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour, IPlayerUIElement {
    PlayerStatus _playerStatusScript;

    Slider _slider;
    float _targetValue, _count;
    bool _begin = false;
    const float speed = 5f;

    void Update() {
        if (_begin) {
            if (_targetValue != _playerStatusScript.Health) {
                UpdateHealth();
            }

            if (_slider.value != _targetValue) {
                Vector2 position = Vector2.Lerp(new Vector2(_slider.value, 0), new Vector2(_targetValue, 0), _count);
                _slider.value = position.x;
                _count += speed * Time.deltaTime;
            } else if (_count != 0) {
                _count = 0;
            }
        }
    }

    public void UpdateHealth() {
        int hp = _playerStatusScript.Health;
        _count = 0;
        if (hp <= 0) {
            _targetValue = 0;
        } else if (hp >= _slider.maxValue) {
            _targetValue = _slider.maxValue;
        } else {
            _targetValue = hp;
        }
    }

    public void Begin(GameObject playerObj) {
        _playerStatusScript = playerObj.GetComponent<PlayerStatus>();
        _slider = GetComponent<Slider>();
        _slider.maxValue = _playerStatusScript.MaxHealth;
        _slider.value = _targetValue = _playerStatusScript.Health;
        _begin = true;
    }
}
