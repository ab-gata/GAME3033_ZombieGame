using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float jumpForce = 5;

    // Components
    PlayerController playerController;
    Rigidbody rigidbody;
    Animator playerAnimator;

    // Movement references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;

    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int isRunningHash = Animator.StringToHash("IsRunning");

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerController.isJumping) return; // No air control
        if (!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed; // if true, do first, if false, do second

        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);

        transform.position += movementDirection;
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();

        // Update animation
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;

        // Update animation
        playerAnimator.SetBool(isRunningHash, playerController.isRunning);
    }

    public void OnJump(InputValue value)
    {
        if (playerController.isJumping) return;

        playerController.isJumping = value.isPressed;
        rigidbody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);

        // Update animation
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping) return;

        playerController.isJumping = false;
    }
}
