using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    const float _speed = 35f;
    const int _dmg = 10;
    const float _deactivateCooldown = 4f;

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void SendProjectile(Vector3 direction, Vector3 position) {
        transform.position = position;
        transform.forward = direction;
        StartCoroutine(DeactivateCooldown());
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerStatus>().TakeDamage(_dmg);
            gameObject.SetActive(false);
        } else if (other.tag == "Ground") {
            gameObject.SetActive(false);
        }
    }

    IEnumerator DeactivateCooldown() {
        yield return new WaitForSeconds(_deactivateCooldown);
        if (gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
        }   
    }
}
