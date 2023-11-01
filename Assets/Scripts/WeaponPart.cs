using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    None,
    Barrel,
    Muzzle,
    Upper,
    Lower,
    Stock,
    Sight
}
public enum PartRarity
{
    None,
    Standard,
    Common,
    Uncommon,
    Rare,
    Legendary
}
public class WeaponPart
{
    string title;
    PartRarity rarity;
    PartType type;

    List<WeaponModifier> modifiers = new List<WeaponModifier>();
    List<WeaponEffect> effects = new List<WeaponEffect>();

    // Generate empty weaponPart
    public WeaponPart() 
    {
        title = "None";
        rarity = PartRarity.None;
        type = PartType.None;
    }

    // Generate weaponPart of rarity
    public WeaponPart(PartRarity _partRarity) 
    {
        title = "None";
        rarity = _partRarity;
        type = PartType.None;
    }

    // Generate weaponPart of type
    public WeaponPart(PartType _partType)
    {
        title = "None";
        rarity = PartRarity.None;
        type = _partType;
    }
}

enum Modifier
{
    None,
    Damage,
    RateOfFire,
    ReloadSpeed,
    AimSpeed,
    CriticalMultiplier,
    VerticalRecoil,
    HorizontalRecoil,
    AmmoReserve
}
public class WeaponModifier
{
    Modifier modifier;
}

public enum Effect
{
    None,
    Ray,
    Projectile,
    SingleShot,
    MultiShot,
    ChargeShot,
    MagazineReload,
    SingleReload,
    BeltAmmo
}
public enum Trigger
{
    None,
    FireDown,
    FireOn,
    FireUp,
    ReloadDown,
    ReloadOn,
    ReloadUp
}
public class WeaponEffect
{

}
