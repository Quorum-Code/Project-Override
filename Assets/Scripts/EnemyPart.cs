using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyPart : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] EnemyLimb enemyLimb = EnemyLimb.NONE; 

    public void Hit(float damage) 
    {
        if (enemyController)
            enemyController.Damage(1.0f, enemyLimb);
    }
}
