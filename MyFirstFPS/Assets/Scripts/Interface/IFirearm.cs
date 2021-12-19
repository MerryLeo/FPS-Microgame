
public interface IFirearm {
    int AmmunitionInClip { get; }
    int Ammunition { get; }
    bool Reloading { get; }
    bool IsReloadingPossible { get; }
    void AddAmmo(int ammo);
    void Shoot();
    void Reload();
}
