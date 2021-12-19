using System.Collections;
using UnityEngine;

public class PlayerGun : MonoBehaviour, IFirearm {
    [SerializeField] 
    GameObject bulletPrefab;
    [SerializeField] 
    ParticleSystem muzzleFlashParticle;
    [SerializeField] 
    Transform frontBarrelTransform;
    [SerializeField]
    Transform cameraTransform;

    public int AmmunitionInClip { get; private set; }
    public int Ammunition { get; private set; }
    public bool Reloading { get; private set; }
    public bool IsReloadingPossible { get; private set; }
    public float FiringCooldown => _shootingCooldown;

    const float _shootingCooldown = 0.2f, _reloadingCooldown = 0.8f, _accuracy = 0.75f, _aimAccuracy = 0.95f;
    const int _maxAmmoPerClip = 10, _initialAmmoNotInClip = 45;
    bool _shootingEnabled;
    ObjectPooling _objectPoolingScript;
    const int _amountToPool = 10;

    void Start() {
        AmmunitionInClip = _maxAmmoPerClip;
        Ammunition = _initialAmmoNotInClip;
        _shootingEnabled = true;
        _objectPoolingScript = GetComponent<ObjectPooling>();
        _objectPoolingScript.BeginPooling(bulletPrefab, _amountToPool);
    }

    // Update is called once per frame
    void Update() {
        IsReloadingPossible = Ammunition > 0 && AmmunitionInClip < _maxAmmoPerClip;
        transform.rotation = transform.parent.rotation;
    }

    public void Shoot() {
        GameObject projectile = _objectPoolingScript.Fetch();
        if (_shootingEnabled && projectile != null) {
            StartCoroutine(ShootingCooldown());
            muzzleFlashParticle.Play();
            RaycastHit hit;
            Vector3 direction = (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit)) ? hit.point - frontBarrelTransform.position : cameraTransform.forward;
            projectile.SetActive(true);
            projectile.GetComponent<PlayerProjectile>().SendProjectile(direction, frontBarrelTransform.position);
            AmmunitionInClip--;
        }
    }

    public void Reload() {
        if (IsReloadingPossible) {
            StartCoroutine(ReloadCooldown());
            int missingAmmoInClip = _maxAmmoPerClip - AmmunitionInClip;
            if (Ammunition - missingAmmoInClip >= 0) {
                Ammunition -= missingAmmoInClip;
                AmmunitionInClip = _maxAmmoPerClip;
            }
            else if (Ammunition - missingAmmoInClip < 0) {
                AmmunitionInClip += Ammunition;
                Ammunition = 0;
            }
        }
    }

    public void AddAmmo(int ammo) {
        Ammunition += ammo;
    }

    IEnumerator ReloadCooldown() {
        Reloading = true;
        yield return new WaitForSeconds(_reloadingCooldown);
        Reloading = false;
    }

    IEnumerator ShootingCooldown() {
        _shootingEnabled = false;
        yield return new WaitForSeconds(_shootingCooldown);
        _shootingEnabled = true;
    }
}
