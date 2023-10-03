using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Weapon classes. Implements adding, removing, and updating weapon parts to be reflected in stats.
/// </summary>
public class FPWeaponController : MonoBehaviour
{
    Camera mainCamera;

    private Weapon activeWeapon;
    private Weapon[] weapons;

    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private GameObject[] muzzleFlashPrefabs;
    [SerializeField] private GameObject[] impactPrefabs;

    private void Awake()
    {
        DefaultWeapon();
    }

    // Generates the starting weapon
    public void DefaultWeapon() 
    {
        // Instance Weapon
        activeWeapon = new();

        // Instance effects
        PartEffect[] lowerEffect = { PartEffect.None };
        PartEffect[] upperEffect = { PartEffect.None };
        PartEffect[] barrelEffect = { PartEffect.FireRay };
        PartEffect[] magazineEffect = { PartEffect.Reload };

        EffectTrigger[] lowerTrigger = { EffectTrigger.None };
        EffectTrigger[] upperTrigger = { EffectTrigger.None };
        EffectTrigger[] barrelTrigger = { EffectTrigger.OnPrimary };
        EffectTrigger[] magazineTrigger = { EffectTrigger.OnReload };

        // Instance Weapon Parts (Lower, Upper, Barrel, Magazine)
        WeaponPart lower = new("lower", PartType.Lower, PartRarity.Standard, lowerEffect, lowerTrigger);
        WeaponPart upper = new("upper", PartType.Upper, PartRarity.Standard, upperEffect, upperTrigger);
        WeaponPart barrel = new("barrel", PartType.Barrel, PartRarity.Standard, barrelEffect, barrelTrigger);
        WeaponPart magazine = new("magazine", PartType.Magazine, PartRarity.Standard, magazineEffect, magazineTrigger);
    }

    public Weapon GetActiveWeapon() 
    {
        return activeWeapon;
    }

    public void ChangeActiveWeapon() 
    {

    }

    public bool AddWeaponPart() 
    {
        return true;
    }

    public bool RemoveWeaponPart() 
    {
        return true;
    }

    public AudioSource audioSource;
    public AudioClip fireClip;

    public Vector3 cameraPoint;
    public Transform weaponEnd;

    public LayerMask enemyMask;

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit cameraHit;
        // Get camera data to calculate direction
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out cameraHit, 100f))
            cameraPoint = cameraHit.point;
        else
            cameraPoint = mainCamera.transform.position + mainCamera.transform.TransformDirection(Vector3.forward) * 25f;

        // Check for input
        if (Input.GetButtonDown("Fire1")) 
        {
            // Call shoot
            Shoot();
        }

        // Shoot line
        Debug.DrawRay(weaponEnd.position, (cameraPoint - weaponEnd.position).normalized * 100f, Color.red);
        // Point line
        Debug.DrawLine(mainCamera.transform.position, cameraPoint, Color.green);
    }

    // TODO remove
    void Shoot() 
    {
        audioSource.PlayOneShot(fireClip, .3f);

        GameObject bullet = Instantiate(bulletPrefab, weaponEnd.position, weaponEnd.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb) 
        {
            rb.AddForce(rb.transform.forward * 150f, ForceMode.Impulse);
        }
    }

    // Generates Weapon effects and stats from parts
    public Weapon RecompileWeapon() 
    {
        Weapon weapon = new();

        return weapon;
    }
}
