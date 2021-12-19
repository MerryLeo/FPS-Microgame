using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour, IFirearm {
    public int AmmunitionInClip { get; private set; }
    public int Ammunition => _ammunitionPerClip;
    public bool Reloading { get; private set; }
    public bool IsReloadingPossible => true;

    [SerializeField]
    GameObject enemyProjectilePrefab;

    [SerializeField]
    Transform frontBarrelTransform;

    [Header("Gun Settings")]
    [SerializeField] [InspectorName("Max Ammo")] [Range(5, 25)]
    int _ammunitionPerClip = 5;
    [Range(1, 5)] [InspectorName("Reloading Cooldown")] [SerializeField]
    float _reloadingCooldown = 2.5f;
    [Range(0.1f, 2)] [InspectorName("Shooting Cooldown")] [SerializeField]
    float _shootingCooldown = 0.8f;
    [Range(5, 15)] [InspectorName("Amount Of Bullet To Pool")] [SerializeField]
    int _amountToPool = 5;
    [Range(0, 100)] [InspectorName("Accuracy")] [SerializeField]
    int accuracy = 75;
    bool _shootingEnabled;
    ObjectPooling _objectPoolingScript;

    // Start is called before the first frame update
    void Start() {
        _shootingEnabled = true;
        Reloading = false;
        AmmunitionInClip = _ammunitionPerClip;
        _objectPoolingScript = GetComponent<ObjectPooling>();
        _objectPoolingScript.BeginPooling(enemyProjectilePrefab, _amountToPool);
    }

    public void Reload() {
        StartCoroutine(ReloadingCooldown());
    }

    public void Shoot() {
        if (_shootingEnabled && !Reloading) {
            GameObject bullet = _objectPoolingScript.Fetch();
            if (bullet != null) {
                bullet.SetActive(true);
                bullet.GetComponent<EnemyProjectile>().SendProjectile(transform.forward, frontBarrelTransform.position);
                StartCoroutine(ShootingCooldown());
            }
        }
    }

    public void ShootAt(Vector3 position) {
        if (_shootingEnabled && !Reloading) {
            GameObject bullet = _objectPoolingScript.Fetch();
            if (bullet != null) {
                Vector3 inaccuracyPosition = Vector3.zero;
                if (accuracy < 100) {
                    inaccuracyPosition = (transform.right * Random.Range(0, 100f - accuracy) + transform.up * Random.Range(0, 100f - accuracy)) / 60f;
                }
                AmmunitionInClip--;
                bullet.SetActive(true);
                bullet.GetComponent<EnemyProjectile>().SendProjectile((position - frontBarrelTransform.position) + inaccuracyPosition, frontBarrelTransform.position);
                StartCoroutine(ShootingCooldown());
            }
        }
    }

    void IFirearm.AddAmmo(int ammo) {
        if (AmmunitionInClip + ammo > _ammunitionPerClip) {
            AmmunitionInClip = _ammunitionPerClip;
        } else {
            AmmunitionInClip += ammo;
        }
    }

    IEnumerator ShootingCooldown() {
        _shootingEnabled = false;
        yield return new WaitForSeconds(_shootingCooldown);
        _shootingEnabled = true;
    }

    IEnumerator ReloadingCooldown() {
        Reloading = true;
        yield return new WaitForSeconds(_reloadingCooldown);
        AmmunitionInClip = _ammunitionPerClip;
        Reloading = false;
    }
}
