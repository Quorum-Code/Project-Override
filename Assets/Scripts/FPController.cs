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
        // Input System Management
        input.Enable();

        // Input init
        input.Player.Jump.started += OnJumpStarted;
        input.Player.Interact.started += OnInteractStarted;
        input.Player.PrimaryFire.started += OnPrimaryStarted;
        input.Player.PrimaryFire.canceled += OnPrimaryCanceled;
    }

    private void OnDisable()
    {
        // Input System Management
        input.Disable();

        // Input deinit
        input.Player.Jump.started -= OnJumpStarted;
        input.Player.Interact.started -= OnInteractStarted;
        input.Player.PrimaryFire.started -= OnPrimaryStarted;
        input.Player.PrimaryFire.canceled -= OnPrimaryCanceled;
    }

    private void FixedUpdate()
    {
        // Physics based movement must be done in FixedUpdate
        PlayerMovement();
    }

    private void Update()
    {
        ProcessInput();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
    }

    #endregion

    #region Input Functions

    private void ProcessInput() 
    {
        // Movement
        movementInput = input.Player.Movement.ReadValue<Vector2>();
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
        // Raycast 
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 2.5f, notPlayer)) 
        {
            // Try to get Pickup component
            Pickup pickup = hit.transform.gameObject.GetComponent<Pickup>();   

            if (pickup != null)
                Debug.Log("Interact object: " + pickup.gameObject.name);
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
