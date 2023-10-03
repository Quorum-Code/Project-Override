using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyLimb { NONE, Head, Torso, Arm, Leg }

public class EnemyController : MonoBehaviour
{
    public void Damage(float amount, EnemyLimb enemyLimb) 
    {
        if (enemyLimb == EnemyLimb.Head)
        {
            Debug.Log("Headshot, 1.5x damage");
        }
        else 
        {
            Debug.Log("Bodyshot, 1.0x damage");
        }
    }
}
