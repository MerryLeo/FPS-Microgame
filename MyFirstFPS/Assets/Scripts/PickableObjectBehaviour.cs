using UnityEngine;

public class PickableObjectBehaviour : MonoBehaviour
{
    [Header("Material Emission Settings")]
    [SerializeField]
    Color minimumEmissionColor;
    [SerializeField]
    Color maximumEmissionColor;
    [SerializeField] [Range(0, 5)]
    float minLightIntensity = 1.5f;
    [SerializeField] [Range(0, 5)]
    float maxLightIntensity = 2.5f;

    [Header("Animation Speed Settings")]
    [SerializeField] [Range(0, 100)]
    float rotationSpeed = 25f;
    [SerializeField] [Range(0, 1)]
    float animationSpeed = 0.5f;

    const float _verticalDistanceModifier = 0.1f;
    float _curveValueRemaped, _animationTime;
    bool _animationIsActive;

    Color _initialColor;
    Vector3 _initialPos;
    Light _light;

    [SerializeField]
    AnimationCurve _curve;
    Material _material;

    // Start is called before the first frame update
    void Start() 
    {
        // Setup the curve
        _curve = new AnimationCurve();
        _curve.AddKey(new Keyframe(0, 1, 0, 0));
        _curve.AddKey(new Keyframe(1, 1, 0, 0));
        _curve.AddKey(new Keyframe(0.5f, -1, 0, 0));
        
        // Get components for the object's light and material
        _light = GetComponentInChildren<Light>();
        _material = GetComponent<Renderer>().material;

        // Initialize the variables
        _initialColor = _material.color;
        _initialPos = transform.position;
        _light.intensity = minLightIntensity;
        _animationTime = 0;

        // Activate the animation variable
        _animationIsActive = true;
    }

    // Update is called once per frame
    void Update() 
    {
        if (_animationIsActive)
        {
            // Value on the animation curve remaped to 0 and 1
            _curveValueRemaped = UtilityClass.Remap(_curve.Evaluate(_animationTime), -1f, 1, 0f, 1f);

            // Update the object's rotation, position, light intensity, and emission color
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            transform.position = _initialPos + Vector3.up * _curve.Evaluate(_animationTime) * _verticalDistanceModifier;
            _light.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, _curveValueRemaped);
            _material.SetColor("_EmissionColor", Color.Lerp(minimumEmissionColor, maximumEmissionColor, _curveValueRemaped));
            _material.SetColor("_Color", _initialColor * Color.black * _curveValueRemaped);

            // Update the animation time variable
            _animationTime += animationSpeed * Time.deltaTime;
            if (_animationTime > 1)
                _animationTime = 0;
        }
    }

    public void SetActive(bool active) 
    {
        if (active) 
        {
            _light.gameObject.SetActive(true);
            _animationIsActive = true;
        }
        else
        {
            _light.gameObject.SetActive(false);
            _animationIsActive = false;
        }
    }
}
