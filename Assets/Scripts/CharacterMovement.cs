
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    #region Variables

    private CharacterController controller;

    private CharacterControls controls;
    private InputAction move;

    [SerializeField] private float speed;
    private float minSpeed = 5f;
    private float maxSpeed = 20f;
    private float averageSpeed;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float offset;
    [SerializeField] private LayerMask groundMask;

    private Transform cam;
    [SerializeField] private float sensitivity;
    [SerializeField] private float rotationSpeed;

    private Vector3 moveValue3;
    private Vector3 moveDirection;
    private Vector2 moveValue;
    private Vector3 velocity;

    private Animator animator;

    private int horizHash;
    private int vertHash;
    private int movingHash;
    private int runningHash;
    private int jumpHash;
    private int ffHash;
    private int fbHash;
    private int speedHash;


    private bool idle = true;
    private bool moving = false;
    private bool running = false;
    private bool jump = false;
    private bool fallingForward = false;
    private bool fallingBackward = false;



    #endregion


    #region MonoBehavior

    private void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        horizHash = Animator.StringToHash("horizontal");
        vertHash = Animator.StringToHash("vertical");
        movingHash = Animator.StringToHash("moving");
        runningHash = Animator.StringToHash("running");
        jumpHash = Animator.StringToHash("jump");
        ffHash = Animator.StringToHash("fallingForward");
        fbHash = Animator.StringToHash("fallingBackward");
        speedHash = Animator.StringToHash("speed");

        speed = minSpeed;
        averageSpeed = (maxSpeed + minSpeed) / 2f;

    }

    private void Awake()
    {
        controls = new CharacterControls();
    }

    private void OnEnable()
    {
        move = controls.Character.Move;
        controls.Character.Jump.started += Jump;
        controls.Character.Enable();
    }

    private void OnDisable()
    {
        controls.Character.Jump.started -= Jump;
        controls.Character.Disable();
    }

    private void Update()
    {
        InputsHandler();

        if (!fallingForward && !fallingBackward)
        {
            Look();
        }

        SpeedController();
    }

    private void FixedUpdate()
    {

        Move();
        Gravity();
        AnimatorController();
    }

    #endregion


    #region Methods

    private void InputsHandler()
    {
        moveValue = move.ReadValue<Vector2>();
    }

    private void Move()
    {

        if (moveValue != Vector2.zero)
        {
            moveValue3 = new Vector3(moveValue.x, 0f, moveValue.y);
        }

        moveDirection = cam.forward * moveValue3.z + cam.right * moveValue3.x;
        moveDirection.y = 0f;

        controller.Move(moveDirection * speed * Time.deltaTime);


    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (IsGrounded() && !fallingBackward && !fallingForward)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            jump = true;
        }
    }

    private void Gravity()
    {

        velocity.y += gravityValue * Time.deltaTime;

        if (IsGrounded() && velocity.y < 0f)
        {
            jump = false;
            velocity.y = -1f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask))
            return true;
        return false;
    }

    private void Look()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up);
        Quaternion targetRotation = Quaternion.LookRotation(camForward, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SpeedController()
    {

        if (moveValue != Vector2.zero)
        {
            idle = false;
            if (speed < maxSpeed)
                speed += 1f * Time.deltaTime;
        }
        else
        {
            if (idle)
                return;

            if (speed < minSpeed)
            {
                speed = minSpeed;
                moveValue3 = Vector3.zero;
                idle = true;
                return;
            }

            speed -= 2f * Time.deltaTime;
        }

        if (jump)
            speed -= 1.5f * Time.deltaTime;

    }

    private void AnimatorController()
    {
        if (moveValue != Vector2.zero)
        {
            animator.SetFloat(horizHash, moveValue.x);
            animator.SetFloat(vertHash, moveValue.y);
        }

        moving = (moveDirection != Vector3.zero);
        animator.SetBool(movingHash, moving);

        running = speed > averageSpeed;
        animator.SetBool(runningHash, running);
        if (running)
        {
            float mma = maxSpeed - averageSpeed;
            float runSpeed = (mma - (maxSpeed - speed)) / mma;
            animator.SetFloat(speedHash, runSpeed + 1f);
        }

        animator.SetBool(jumpHash, jump);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp"))
        {
            if (fallingForward)
            {
                fallingForward = false;
                animator.SetBool(ffHash, false);
            }
            else if (fallingBackward)
            {
                fallingBackward = false;
                animator.SetBool(fbHash, false);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        FallingAnimation(hit);

    }

    private void FallingAnimation(ControllerColliderHit hit)
    {

        if (fallingBackward || fallingForward)
            return;

        if (!moving)
            return;

        if (hit.gameObject.layer == 7)
        {
            speed -= 1f * Time.deltaTime;

            if (speed <= (averageSpeed + minSpeed) / 2f)
                return;

            float vert = animator.GetFloat(vertHash);
            float horiz = animator.GetFloat(horizHash);

            Vector3 characterSize = controller.bounds.size;
            Vector3 obstacleSize = hit.collider.bounds.size;

            Debug.Log(characterSize);
            Debug.Log(obstacleSize);

            if (obstacleSize.y <= characterSize.y / 4f)
            {
                if (vert > 0)
                    fallingForward = true;
                else if (vert < 0)
                    fallingBackward = true;
                else if (horiz > 0)
                {
                    fallingForward = true;
                    TurnRight();
                }

                else if (horiz < 0)
                {
                    fallingForward = true;
                    TurnLeft();
                }

                else
                    return;

            }
            else
            {
                if (vert > 0)
                    fallingBackward = true;
                else if (vert < 0)
                    fallingForward = true;
                else if (horiz > 0)
                {
                    fallingBackward = true;
                    TurnRight();
                    
                }
                else if (horiz < 0)
                {
                    fallingBackward = true;
                    TurnLeft();
                    
                }
                else
                    return;
            }

            speed = 0f;

            if (fallingForward)
                animator.SetBool(ffHash, true);
            else if (fallingBackward)
                animator.SetBool(fbHash, true);
        }

    }


    private void TurnLeft()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, -90f, 0f);

        transform.rotation = targetRotation;
    }

    private void TurnRight()
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 90f, 0f);

        transform.rotation = targetRotation;
    }


    #endregion
}
