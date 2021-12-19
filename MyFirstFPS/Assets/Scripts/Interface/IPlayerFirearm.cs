// Interface for the player firearms
public interface IPlayerFirearm
{
    int AmmunitionInClip { get; }
    int AmmunitionOutOfClip { get; }
    int TotalAmmunitionInGun { get; }
    float BulletSpread { get; }
    float Firerate { get; }
    bool Reloading { get; }
    bool Shooting { get; }
    bool IsReloadingPossible { get; }
    bool IsShootingPossible { get; }
    void AddAmmo(int ammo);
    void Shoot();
    void Reload();
}
