using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{

    #region General Variables
    [Header("Movement & Look")]
    [SerializeField] GameObject camHolder;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float crouchSpeed = 3.0f;
    [SerializeField] float sprintSpeed = 8.0f;
    [SerializeField] float maxForce = 1.0f; //fuerza maxima de aceleracion
    [SerializeField] float sensitivity = 0.1f;


    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    #endregion


    //Variables de autoreferencia

    Rigidbody rb;

    //Variables de input

    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    void Start()
    {
        //Lock del cursor

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    // Update is called once per frame
    void Update()
    {

      



    }

    #region InputMetods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {

    }
    public void OnCrouch(InputAction.CallbackContext context)
    {

    }
    public void OnSprint(InputAction.CallbackContext context)
    {

    }







        #endregion





}
