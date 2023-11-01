using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

enum buttonState { None, Down, Held, Up }

public class FPController : MonoBehaviour
{
    #region Attributes

    [Header("Input")]
    private CustomInput input = null;

    [Header("Movement Settings")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;

    [Header("Camera")]
    public Transform orientation;
    public Transform mainCamera;

    // Movement input
    Vector3 moveDireciton;
    Vector2 movementInput = Vector2.zero;

    buttonState jumpInput = buttonState.None;
    buttonState primaryInput = buttonState.None;

    bool canMelee = true;
    bool canJump = true;

    Rigidbody rb;

    public LayerMask notPlayer;

    [SerializeField] FPWeaponController weaponController;
    Weapon weapon;

    // Interactables
    [SerializeField] GameObject inventoryCanvas;
    GameObject interactable;
    List<WeaponPart> weaponParts = new List<WeaponPart>();

    #endregion

    #region GameObject Functions

    private void Awake()
    {
        input = new CustomInput();
    }

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;

        weapon = weaponController.GetActiveWeapon();
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void FixedUpdate()
    {
        // Physics based movement must be done in FixedUpdate
        PlayerMovement();

        // Check if player is looking at interactable
        InteractCheck();
    }

    private void Update()
    {
        ProcessInput();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
    }

    public bool addPartToInventory(WeaponPart weaponPart) 
    {
        if (weaponParts.Count >= 5)
        {
            return false;
        }
        else 
        {
            weaponParts.Add(weaponPart);
            return true;
        }
    }

    #endregion

    #region Input Functions

    private void EnableInput() 
    {
        // Input System Management
        input.Enable();

        // Input init
        input.Player.Jump.started += OnJumpStarted;
        input.Player.PrimaryFire.started += OnPrimaryStarted;
        input.Player.PrimaryFire.canceled += OnPrimaryCanceled;

        input.Player.InteractPress.started += OnInteractStarted;
        input.Player.InteractPress.performed += OnInteractTap;
        input.Player.InteractHold.performed += OnInteractHold;
        input.Player.InteractPress.canceled += OnInteractCanceled;

        input.Player.Inventory.performed += OnInventoryPerformed;
    }

    private void DisableInput() 
    {
        // Input System Management
        input.Disable();

        // Input deinit
        input.Player.Jump.started -= OnJumpStarted;
        input.Player.PrimaryFire.started -= OnPrimaryStarted;
        input.Player.PrimaryFire.canceled -= OnPrimaryCanceled;

        input.Player.InteractPress.started -= OnInteractStarted;
        input.Player.InteractPress.performed -= OnInteractTap;
        input.Player.InteractHold.performed -= OnInteractHold;
        input.Player.InteractPress.canceled -= OnInteractCanceled;

        input.Player.Inventory.performed -= OnInventoryPerformed;
    }

    private void ProcessInput() 
    {
        // Movement
        movementInput = input.Player.Movement.ReadValue<Vector2>();

        // Debug.Log("Interact: " + input.Player.Interact.ReadValue<float>());
    }

    private void OnJumpStarted(InputAction.CallbackContext value)
    {
        // Check if can Jump
        if (canJump && isGrounded)
        {
            canJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MeleeStarted()
    {

    }

    private void OnInteractStarted(InputAction.CallbackContext value) 
    {
        Debug.Log("Interact Started");

        // Raycast from camera

        // Raycast 
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 2.5f, notPlayer))
        {
            // Try to get Pickup component
            Pickup pickup = hit.transform.gameObject.GetComponent<Pickup>();

            // Store pickup if found
            if (pickup != null) 
            {
                Debug.Log("Interact object: " + pickup.gameObject.name);
                interactable = pickup.gameObject;
            }
                
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext value)
    {
        
    }

    private void OnInteractPerformed(InputAction.CallbackContext value) 
    {
        
    }

    private void OnInventoryPerformed(InputAction.CallbackContext value)
    {
        // Close Inventory
        if (inventoryCanvas.activeSelf)
        {
            inventoryCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        // Open Inventory
        else 
        {
            inventoryCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnInteractTap(InputAction.CallbackContext value)
    {
        if (interactable) 
        {
            Pickup pickup = interactable.GetComponent<Pickup>();
            if (pickup) 
            {
                pickup.PickupObject(this);
            }
        }
    }

    private void OnInteractHold(InputAction.CallbackContext value) 
    {
        if (interactable)
        {
            Pickup pickup = interactable.GetComponent<Pickup>();
            if (pickup)
            {
                pickup.EquipObject(this);
            }
        }
    }

    private void OnPrimaryStarted(InputAction.CallbackContext value) 
    {
        if (primaryInput != buttonState.Down)
        {
            if (weapon.startFire != null) 
                weapon.startFire();
            primaryInput = buttonState.Held;
        }
        else 
        {
            primaryInput = buttonState.Held;
            if (weapon.whileFire != null)
                weapon.whileFire();
        }
    }

    private void OnPrimaryCanceled(InputAction.CallbackContext value)
    {
        primaryInput = buttonState.Up;
        if (weapon.endFire != null)
            weapon.endFire();
        primaryInput = buttonState.None;
    }

    #endregion

    #region Movement Functions

    private void MyInput() 
    {
        // Melee Input
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            // Get point in front of camera
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 2.5f, notPlayer)) 
            {

                // Get objects with small check sphere
                Collider[] colliders = Physics.OverlapSphere(hit.point, .25f);

                // Push objects back
                foreach (Collider collider in colliders)
                {
                    Rigidbody crb = collider.GetComponent<Rigidbody>();
                    if (crb) 
                    {
                        crb.AddForceAtPosition(mainCamera.forward * 5f, hit.point, ForceMode.Impulse);

                        //crb.AddExplosionForce(500f, hit.point, 25f, 0f);
                    }
                }
            }
        }
    }

    private void InteractCheck() 
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 2.5f, notPlayer))
        {
            // Try to get Pickup component
            Pickup pickup = hit.transform.gameObject.GetComponent<Pickup>();

            // Enable pickup/equip text
            if (pickup != null)
                ;
            // Disable pickup/equip text
            else
                ;
        }
        // if not interacting
        // Raycast from camera
        // if object
        // check if interactable
        // if holding interact start timer
    }

    private void PlayerMovement() 
    {
        moveDireciton = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        if(isGrounded)
            rb.AddForce(moveDireciton.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDireciton.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl() 
    {
        // Velocity Limiter
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        // Drag control
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void ResetJump() { canJump = true; }
    private void ResetMelee() { canMelee = true; }

    #endregion
}
