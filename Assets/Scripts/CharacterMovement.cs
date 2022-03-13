
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    #region Variables

    private CharacterController controller;
    private CharacterControls controls;

    private InputAction move;

    private float speed;
    private float minSpeed = 5f;
    private float maxSpeed = 15f;
    private bool isStoped = true;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float offset;
    [SerializeField] private LayerMask groundMask;

    private Transform cam;
    [SerializeField] private float sensitivity;
    [SerializeField] private float rotationSpeed;

    private Vector3 moveValue3;
    private Vector3 moveDirection;
    //private Vector3 currentDirection;
    private Vector2 moveValue;
    private Vector3 velocity;

    private Animator animator;


    #endregion


    #region MonoBehavior

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

    private void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        speed = minSpeed;

        Vector3 a = new Vector3(1f, 2f, 3f);
        Vector3 b = new Vector3(1f, 2f, 3f);

        Debug.Log(a + b);
    }

    private void Update()
    {
        InputsHandler();
        Look();
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
        if (IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            animator.SetBool("jump", true);
        }

    }

    private void Gravity()
    {

        velocity.y += gravityValue * Time.deltaTime;

        if (IsGrounded() && velocity.y < 0f)
        {
            velocity.y = -2f;
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

        //Debug.Log(speed);

        if (moveValue != Vector2.zero)
        {
            isStoped = false;
            if (speed < maxSpeed)
                speed += 1f * Time.deltaTime;
        }
        else
        {
            if (isStoped)
                return;

            if (speed < minSpeed)
            {
                speed = minSpeed;
                moveValue3 = Vector3.zero;
                isStoped = true;
                return;
            }

            speed -= 2f * Time.deltaTime;
        }
    }

    private void AnimatorController()
    {
        animator.SetFloat("horizontal", moveValue.x);
        animator.SetFloat("vertical", moveValue.y);

        if (moveValue != Vector2.zero)
            animator.SetBool("walking", true);
        else
            animator.SetBool("walking", false);

        if (speed > 10f)
            animator.SetBool("running", true);
        else
            animator.SetBool("running", false);
        
        
        if (IsGrounded() && animator.GetBool("jump"))
            animator.SetBool("jump", false);
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(spherePos, controller.radius - 0.05f);
    //}



    #endregion
}
