using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    public int Health { get; private set; } = 100;
    public int MaxHealth { get; } = 100;
    public bool IsDead { get; private set; } = false;
    public void TakeDamage(int dmg) {
        Health -= dmg;
        if (Health <= 0) {
            Die();
        }
    }

    public void AddHealth(int hp) {
        if (Health + hp > MaxHealth) {
            Health = MaxHealth;
        } else {
            Health += hp;
        }
    }

    void Die() {
        IsDead = true;
    }
}
