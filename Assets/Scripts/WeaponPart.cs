using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType { None, Barrel, Upper, Lower, Magazine, Muzzle, Rest, Scope, Tactical }
public enum PartRarity { None, Standard, Common, Uncommon, Rare, Legendary }
public enum EffectTrigger { None, OnPrimary, WhilePrimary, EndPrimary, 
                                  OnSecondary, WhileSecondary, EndSecondary,
                                  OnReload, WhileReload, EndReload,
                                  Fire, Aim, Reload}
public enum PartEffect { None, TriggerPull, TriggerRelease, TriggerCharge, FireRay, FireProjectile, FireSingle, FireShot, Reload }

public class WeaponPart
{
    string title;

    public PartType partType { get; private set; }
    public PartRarity partRarity { get; private set; }
    public PartEffect[] partEffects { get; private set; }
    public EffectTrigger[] effectTriggers { get; private set; }

    public WeaponPart(string _title, PartType _partType, PartRarity _partRarity, PartEffect[] _partEffects, EffectTrigger[] _effectTriggers) 
    {
        title = _title;
        partType = _partType;
        partRarity = _partRarity;
        partEffects = _partEffects;
        effectTriggers = _effectTriggers;
    }

    public virtual void PickUp() 
    {

    }
}
