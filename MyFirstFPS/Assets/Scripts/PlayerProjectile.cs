using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    float _speed = 85f;
    int _dmg = 10;
    float _deactivateCooldown = 2f;

    // Update is called once per frame
    void Update() 
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void SendProjectile(Vector3 direction, Vector3 position) 
    {
        transform.forward = direction;
        transform.position = position;
        StartCoroutine(DeactivateCooldown());
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Enemy") 
        {
            other.GetComponent<EnemyStatus>().TakeDamage(_dmg);

            //Vector3 knockbackDirection = new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z);
            //other.GetComponent<Enemy>().Knockback(knockbackDirection, _knockbackStrength);
            gameObject.SetActive(false);
        }
        else if (other.tag == "Ground") 
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator DeactivateCooldown() 
    {
        yield return new WaitForSeconds(_deactivateCooldown);
        if (gameObject.activeInHierarchy) 
        {
            gameObject.SetActive(false);
        }
    }
}
