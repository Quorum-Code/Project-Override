using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartPickup : Pickup
{
    public WeaponPart weaponPart;

    private void Awake()
    {
        isEquipable = true;
        weaponPart = new WeaponPart();
    }

    public override void PickupObject(FPController fpcontroller)
    {
        if (fpcontroller.addPartToInventory(weaponPart)) 
        {
            // Pass player the weaponPart class
            Debug.Log("Weapon part picked up");

            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Cannot add, inventory full");
        }
    }

    public override void EquipObject(FPController fpcontroller) 
    {
        Debug.Log("Weapon part equipped!");
    }
}
