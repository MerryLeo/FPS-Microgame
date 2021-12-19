using System.Collections;
using UnityEngine;

public class PlayerAssaultRifle : MonoBehaviour, IPlayerFirearm
{
    public int AmmunitionInClip { get; private set; }
    public int AmmunitionOutOfClip { get; private set; }

    public int TotalAmmunitionInGun { get { return AmmunitionInClip + AmmunitionOutOfClip; } }

    public float BulletSpread => bulletSpread;
    public float Firerate => fireRate;

    public bool Reloading { get; private set; }
    public bool Shooting { get; private set; }

    public bool IsReloadingPossible { get; private set; }
    public bool IsShootingPossible { get; private set; }

    [Header("Firearm Settings")]
    [SerializeField]
    float bulletSpread = 0.25f;

    [SerializeField]
    float fireRate = 0.2f;

    [SerializeField]
    int maxAmmunitionPerClip = 10;

    [SerializeField]
    int initialAmmo = 45;

    [SerializeField]
    float reloadingCooldown = 0.8f;

    [Header("Game Objects")]
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject fakeBulletPrefab;
    [SerializeField]
    Transform muzzleTransform;

    // Private fields
    ObjectPooling _bulletPoolingScript;
    ObjectPooling _fakeBulletPoolingScript;

    Recoil _recoilScript;

    Transform _cameraTransform;
    Camera _camera;

    const float _minDstFromCamera = 8f, _maxDstFromCamera = 100f, _defaultDstFromCamera = 45f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the amount of ammunition
        AmmunitionInClip = maxAmmunitionPerClip;
        AmmunitionOutOfClip = 0;
        AddAmmo(initialAmmo);

        Shooting = Reloading = false;

        _cameraTransform = transform.parent;
        _camera = GameObject.Find("FPS Camera").GetComponent<Camera>();

        // Start the pooling lists
        _bulletPoolingScript = gameObject.AddComponent<ObjectPooling>();
        _fakeBulletPoolingScript = gameObject.AddComponent<ObjectPooling>();

        LayerMask _invisibleLayer = LayerMask.NameToLayer("Invisible");
        LayerMask _fpsViewLayer = LayerMask.NameToLayer("FPSView");

        _bulletPoolingScript.BeginPooling(bulletPrefab, maxAmmunitionPerClip, _invisibleLayer);
        _fakeBulletPoolingScript.BeginPooling(fakeBulletPrefab, maxAmmunitionPerClip, _fpsViewLayer);

        // Initialize the recoil script
        _recoilScript = gameObject.AddComponent<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        IsReloadingPossible = AmmunitionOutOfClip > 0 && AmmunitionInClip < maxAmmunitionPerClip && !Reloading;
        IsShootingPossible = AmmunitionInClip > 0 && !Reloading && !Shooting;
    }

    public void Shoot()
    {
        GameObject _bullet = _bulletPoolingScript.Fetch();
        GameObject _fakeBullet = _fakeBulletPoolingScript.Fetch();
        if (IsShootingPossible && _bullet != null && _fakeBullet != null)
        {
            _recoilScript.Activate(fireRate);
            StartCoroutine(ShootingCooldown());

            // Send the invisible bullet which can hit enemies
            _bullet.SetActive(true);
            PlayerProjectile bullet = _bullet.GetComponent<PlayerProjectile>();
            bullet.SendProjectile(_cameraTransform.forward, _cameraTransform.position);

            // Send the fake bullet that only the player can see
            _fakeBullet.SetActive(true);
            FakeProjectile fakeBullet = _fakeBullet.GetComponent<FakeProjectile>();
            Vector3 from, to;
            from = muzzleTransform.position;
            
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo);
            float dstFromCamera = _defaultDstFromCamera;
            if (hit)
            {
                dstFromCamera = Vector3.Distance(hitInfo.point, _cameraTransform.position);
                dstFromCamera = Mathf.Clamp(dstFromCamera, _minDstFromCamera, _maxDstFromCamera);
            }
            to = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).GetPoint(dstFromCamera);

            fakeBullet.SendProjectile(from, to);

            // Reduce the amount of ammunition in the clip
            AmmunitionInClip--;
        }
    }

    public void Reload()
    {
        if (IsReloadingPossible)
        {
            StartCoroutine(ReloadingCooldown());
            int _missingAmmoInClip = maxAmmunitionPerClip - AmmunitionInClip;
            if (AmmunitionOutOfClip >= _missingAmmoInClip)
            {
                AmmunitionOutOfClip -= _missingAmmoInClip;
                AmmunitionInClip = maxAmmunitionPerClip;
            }
            else if (AmmunitionOutOfClip < _missingAmmoInClip)
            {
                AmmunitionInClip += AmmunitionOutOfClip;
                AmmunitionOutOfClip = 0;
            }
        }
    }

    public void AddAmmo(int ammo)
    {
        AmmunitionOutOfClip += ammo;
    }

    IEnumerator ShootingCooldown()
    {
        Shooting = true;
        yield return new WaitForSeconds(fireRate);
        Shooting = false;
    }

    IEnumerator ReloadingCooldown()
    {
        Reloading = true;
        yield return new WaitForSeconds(reloadingCooldown);
        Reloading = false;
    }
}
