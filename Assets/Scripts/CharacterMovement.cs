using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    #region Variables

    private CharacterController characterController;
    private InputMaster inputMaster;

    [SerializeField] Transform cam;
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;

    private Vector3 playerMove;
    private Vector2 mouseDelta;
    private float xRotation;

    #endregion


    #region MonoBehavior

    private void Awake()
    {
        inputMaster = new InputMaster();
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void Start()
    {
        

        inputMaster.Character.Movement.performed += ctx =>
        {
            playerMove = new Vector3(ctx.ReadValue<Vector2>().x, playerMove.y, ctx.ReadValue<Vector2>().y);
        };
        inputMaster.Character.Movement.canceled += ctx =>
        {
            playerMove = new Vector3(ctx.ReadValue<Vector2>().x, playerMove.y, ctx.ReadValue<Vector2>().y);
        };

        inputMaster.Character.MouseDelta.performed += ctx =>
        {
            mouseDelta = ctx.ReadValue<Vector2>();
        };
        inputMaster.Character.MouseDelta.canceled += ctx =>
        {
            mouseDelta = ctx.ReadValue<Vector2>();
        };

        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Look();
        if (inputMaster.Character.Jump.triggered)
            Debug.Log("Jump");
    }

    private void FixedUpdate()
    {
        
        Move();
        Jump();
    }

    #endregion


    #region Methods

    private void Move()
    {
        Vector3 moveVec = transform.TransformDirection(playerMove);
        characterController.Move(moveVec * speed * Time.deltaTime);
    }

    private void Jump()
    {
        
    }

    private void Look()
    {
        xRotation -= mouseDelta.y * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 45f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity * Time.deltaTime);

    }

    #endregion
}
