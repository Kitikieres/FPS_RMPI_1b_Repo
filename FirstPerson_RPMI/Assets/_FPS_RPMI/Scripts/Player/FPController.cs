using JetBrains.Annotations;
using UnityEditor.Timeline;
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

    [Header("Jump and GroundCheck")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;


    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    #endregion


    //Variables de autoreferencia

    Rigidbody rb;
    Animator anim;

    //Variables de input

    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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


        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);


    }

    private void FixedUpdate()
    {

        Movement();
    }

    private void LateUpdate()
    {

        CameraLook();
    }
    void CameraLook()
    {
        // rotacion del personaje horizontal
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        // rotacion de la camara
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.localEulerAngles = new Vector3(lookRotation, 0f, 0f);
    }

    void Movement()
    {
        //definir 2 vectores de aceleracion
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);
        //direccion a alcanzar le multiplicamos la velocidad
        targetVelocity *= isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : speed;
        //convertirlo en eje mundial
        targetVelocity = transform.TransformDirection(targetVelocity);

        //calcular el cambio de velocidad
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);
        //Aplicacion del movimiento (DIRECION MAS ACELERACION)
        rb.AddForce(velocityChange, ForceMode.VelocityChange);


    }
    void Jump()
    {
        if (isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
        if (context.performed) Jump();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            anim.SetBool("isCrouching", isCrouching);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !isCrouching) isSprinting = true;
        if (context.canceled) isSprinting = false; 
    }







        #endregion





}
