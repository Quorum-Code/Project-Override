using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    Weapon weapon;
    [SerializeField] Mesh weaponMesh;

    private void Start()
    {
        
    }

    public void setWeapon(Weapon w) 
    {

    }

    public override void PickupObject(FPController fpcontroller) 
    {
        // Pass player the weapon class

        // Pass player the weapon mesh



        Destroy(this.gameObject);
    }

    public override void EquipObject(FPController fpcontroller)
    {
        
    }
}
