using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    Weapon weapon;
    [SerializeField] Mesh weaponMesh;

    private void Start()
    {
        weapon = new Weapon();
    }

    public override void PickupObject(FPController fpcontroller) 
    {
        // Pass player the weapon class

        // Pass player the weapon mesh

        Destroy(this.gameObject);
    }
}
