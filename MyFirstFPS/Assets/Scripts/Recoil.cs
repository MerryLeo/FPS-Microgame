using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple class to move back and forward an object
public class Recoil : MonoBehaviour
{
    Vector3 _offset;

    [SerializeField]
    AnimationCurve _curve;

    float _count, _speed;
    const float _dst = 0.15f;
    bool _enabled;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Initialize the variables
        _offset = transform.localPosition;
        _count = 0;
        _enabled = false;

        // Set the animation curve
        Keyframe[] keys = { new Keyframe(0, 0, 1, -8), new Keyframe(0.25f, -1), new Keyframe(1, 0) };
        _curve = new AnimationCurve(keys);

    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled)
        {
            float _value = _curve.Evaluate(_count);
            transform.localPosition = _offset + Vector3.forward * _value * _dst;
            _count += _speed * Time.deltaTime;

            if (_count > 1)
            {
                _enabled = false;
                _count = 0;
                transform.localPosition = _offset;
            }
        }
    }

    /// <summary>
    /// Activate the recoil animation.
    /// </summary>
    /// <param name="time">Amount of time in seconds it will take for the animation to finish</param>
    public void Activate(float time)
    {
        _count = 0;
        _speed = 1f / (float)time;
        _enabled = true;
    }
}
