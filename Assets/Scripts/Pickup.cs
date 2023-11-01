using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Pickup : MonoBehaviour
{
    public bool isEquipable { get; protected set; } = false;

    public abstract void PickupObject(FPController fpcontroller);
    public abstract void EquipObject(FPController fpcontroller);
}
