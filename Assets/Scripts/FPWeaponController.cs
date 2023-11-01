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

    // Recoil vars
    Vector3 cameraRotation;
    Vector3 targetRotation;
    [SerializeField] float xRecoil = -5f;
    [SerializeField] float yRecoil = .2f;
    [SerializeField] float snappiness = 3f;
    [SerializeField] float returnSpeed = 1f;

    [SerializeField] ParticleSystem[] muzzleFlashes;

    // Aim animation
    IEnumerator aimingAnimation;
    [SerializeField] GameObject weaponModel;
    [SerializeField] Transform hipLoc;
    [SerializeField] Transform aimLoc;
    [SerializeField] Transform hipOffset;
    [SerializeField] Transform aimOffset;

    private void Awake()
    {
        DefaultWeapon();
    }

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

        // weaponModel.transform.localPosition = aimLoc.localPosition - aimOffset.localPosition;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (aimingAnimation != null) 
                StopCoroutine(aimingAnimation);
            
            aimingAnimation = AimIn();
            StartCoroutine(aimingAnimation);
        }
        else if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (aimingAnimation != null)
                StopCoroutine(aimingAnimation);

            aimingAnimation = AimOut();
            StartCoroutine(aimingAnimation);
        }

        // Debug.DrawLine(aimLoc.position + aimOffset.position, hipLoc.position + hipOffset.position, Color.magenta);

        UpdateRecoil();
    }

    // Generates the starting weapon
    public void DefaultWeapon() 
    {
        // Instance Weapon
        activeWeapon = new();

        // Instance effects
        /*PartEffect[] lowerEffect = { PartEffect.None };
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
        WeaponPart magazine = new("magazine", PartType.Magazine, PartRarity.Standard, magazineEffect, magazineTrigger);*/
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

        // Recoil
        AddRecoil();

        // Muzzle Flash
        foreach (ParticleSystem flash in muzzleFlashes) 
        {
            flash.Play();
        }
    }

    // Generates Weapon effects and stats from parts
    public Weapon RecompileWeapon() 
    {
        Weapon weapon = new();

        return weapon;
    }

    private void UpdateRecoil() 
    {
        targetRotation = Vector3.Slerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        cameraRotation = Vector3.Slerp(cameraRotation, targetRotation, snappiness * Time.deltaTime);
        mainCamera.transform.localRotation = Quaternion.Euler(cameraRotation);
    }

    private void AddRecoil() 
    {
        targetRotation += new Vector3(xRecoil * Random.Range(.9f, 1.1f), Random.Range(-yRecoil, yRecoil), 0f);
    }

    private void posTest() 
    {

    }

    IEnumerator AimIn() 
    {
        Vector3 hipTarget = hipLoc.localPosition - hipOffset.localPosition;
        Vector3 aimTarget = aimLoc.localPosition - aimOffset.localPosition;

        float timeToAim = .25f;

        float targetMagnitude = Vector3.Distance(aimTarget, hipTarget);
        float actualMagnitude = Vector3.Distance(aimTarget, weaponModel.transform.localPosition);

        float t = (1 - actualMagnitude / targetMagnitude) * timeToAim;
        
        // Lerp model to point
        while (t < timeToAim) 
        {
            t += Time.deltaTime;
            weaponModel.transform.localPosition = Vector3.Lerp(hipTarget, aimTarget, t / timeToAim);
            yield return null;
        }

        // Snap model to end
        weaponModel.transform.localPosition = aimTarget;
    }

    IEnumerator AimOut() 
    {
        Vector3 hipTarget = hipLoc.localPosition - hipOffset.localPosition;
        Vector3 aimTarget = aimLoc.localPosition - aimOffset.localPosition;

        float timeToAim = .25f;

        float targetMagnitude = Vector3.Distance(hipTarget, aimTarget);
        float actualMagnitude = Vector3.Distance(hipTarget, weaponModel.transform.localPosition);

        float t = (1 - actualMagnitude / targetMagnitude) * timeToAim;

        // Lerp model to point
        while (t < timeToAim)
        {
            t += Time.deltaTime;
            weaponModel.transform.localPosition = Vector3.Lerp(aimTarget, hipTarget, t / timeToAim);
            yield return null;
        }

        // Snap model to end
        weaponModel.transform.localPosition = hipTarget;
    }
}
