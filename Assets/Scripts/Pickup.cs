using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Pickup : MonoBehaviour
{
    public abstract void PickupObject(FPController fpcontroller);
}
