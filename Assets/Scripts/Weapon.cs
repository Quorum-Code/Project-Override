using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public Transform firePoint;
    public GameObject fireProjectile;

    private List<WeaponPart> parts;

    // Weapon Actions
    public Action startFire;
    public Action whileFire;
    public Action endFire;

    public Action startReload;
    public Action whileReload;
    public Action endReload;

    public Action startAim;
    public Action whileAim;
    public Action endAim;

    float TriggerValue = 0f;
    float TriggerThreshold = 0f;

    // Weapon Stats 
    float Damage = 1f;
    float Velocity = 25f;
    float Range = 100f;
    float Spread = .5f;
    
    int Shots = 1;
    int MaxMagCount = 20;
    int MaxAmmoCount = 200;

    int curMagCount = 20;
    int curAmmoCount = 200;

    AudioClip onFireClip;

    private void Start()
    {
        // Initialize Actions
        startFire = EmptyAction;
        whileFire = EmptyAction;
        endFire = EmptyAction;

        startReload = EmptyAction;
        whileReload = EmptyAction;
        endReload = EmptyAction;

        startAim = EmptyAction;
        whileAim = EmptyAction;
        endAim = EmptyAction;
    }

    public void AddPart(WeaponPart weaponPart) 
    {
        // Check is not null
        if (weaponPart == null || weaponPart.partType == PartType.None)
        {
            return;
        }
        else
        {
            // Check parts
            foreach (WeaponPart part in parts) 
            {
                if (part.partType == weaponPart.partType) 
                {
                    return;
                }
            }

            // Add part
            parts.Add(weaponPart);
        }
    }

    public void AddParts(WeaponPart[] weaponParts) 
    {
        foreach (WeaponPart part in weaponParts) 
        {
            AddPart(part);
        }
    }

    private void RemovePart() 
    {

    }

    private void RebuildWeapon() 
    {

    }

    private void ProcessEffect() 
    {

    }

    /* ============================
     * Dynamic Function Definitions
     * ============================ */
    public void EmptyAction() { }

    public void TriggerChargeRelease() 
    {
        if (Input.GetButton("Fire1"))
        {
            TriggerValue += Time.deltaTime;
        }
        else if (TriggerValue != 0f) 
        {
            //fireAction();
        }
    }

    public void TriggerChargeThresh() 
    {
        // Charge Trigger Value
        if (Input.GetButton("Fire1"))
        {
            TriggerValue += Time.deltaTime;
        }
        // If TriggerValue over TriggerThreshold then fireAction
        else if (TriggerValue >= TriggerThreshold) 
        {
            //fireAction();
        }
    }

    public void SimpleReload() 
    {
        Debug.Log("Simple aim, needs to be implemented");

        // Input Check
        if (Input.GetKeyDown(KeyCode.R) && curMagCount < MaxMagCount) 
        {
            // Full reload
            if (curAmmoCount > MaxMagCount - curMagCount)
            {
                // Update ammo
                curAmmoCount -= (MaxMagCount - curMagCount);
                curMagCount = MaxMagCount;

                // Activate reload anim
                // Start use delay
            }
            // Partial reload
            else if (curAmmoCount > 0) 
            {
                // Update ammo
                curMagCount = curAmmoCount;
                curAmmoCount = 0;

                // Activate reload anim
                // Start use delay
            }
        }
    }

    public void FireRay()
    {
        Debug.Log("Fired Ray!");
    }

    public void BaseAim() 
    {
        Debug.Log("Base aim, needs to be implemented");
    }

    public void BaseReload() 
    {
        Debug.Log("Base reload");

        if (curMagCount < MaxMagCount)
        {
            // Full reload
            if (curAmmoCount > MaxMagCount - curMagCount)
            {
                // Update ammo
                curAmmoCount -= (MaxMagCount - curMagCount);
                curMagCount = MaxMagCount;

                // Activate reload anim
                // Start use delay
            }
            // Partial reload
            else if (curAmmoCount > 0)
            {
                // Update ammo
                curMagCount = curAmmoCount;
                curAmmoCount = 0;

                // Activate reload anim
                // Start use delay
            }
        }
    }
}
