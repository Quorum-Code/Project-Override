using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevWeaponPartDropper : MonoBehaviour
{
    public GameObject dropPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            Debug.Log("Spawning weapon part");

            // Generate weapon part
            WeaponPart wp = new WeaponPart();

            // Instance prefab
            GameObject g = Instantiate(dropPrefab, this.transform);

            // Attach weapon part to prefab
            Pickup p = g.GetComponent<WeaponPartPickup>();
            if (p != null ) 
            {
                
            }
        }
    }
}
