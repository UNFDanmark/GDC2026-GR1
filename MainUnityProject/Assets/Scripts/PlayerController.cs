using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    
    [Header("Movement")]
    public InputActionReference actionMovement;   // vector2
    public InputActionReference actionJump;       // button
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public AudioSource audioSourceMove;
    public AudioSource audioSourceJump;
    Rigidbody rb;
    bool canMove = true;
    
    [Header("Dash")]
    public InputActionReference actionDash;       // button
    public float dashSpeed = 10f;
    public float dashDisablesMovementFor = 0.5f;
    public float dashCooldown = 2f;
    public Volume motionBlurVolume;
    public AudioSource audioSourceDash;
    bool canDash = true;
    
    [Header("Camera")]
    public InputActionReference actionLook;       // vector2
    public GameObject cameraGameObject;
    float cameraSensX = 0.5f;
    float cameraSensY = 0.5f;
    Camera camera;
    
    [Header("Attack")]
    public InputActionReference actionAttack;     // button
    public GameObject attackBox;
    public float attackLifetime = 0.2f;
    public float attackCooldown = 0.5f;
    public AudioSource audioSourceAttack;
    bool canAttack = true;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = cameraGameObject.GetComponent<Camera>();
        actionMovement.action.Enable();
        actionLook.action.Enable();
        actionJump.action.Enable();
        actionJump.action.started += Jump;
        actionDash.action.Enable();
        actionDash.action.started += Dash;
        actionAttack.action.Enable();
        actionAttack.action.started += Attack;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        SettingsManager.SettingsChangedEvent.AddListener(ReloadSettings);
        ReloadSettings();
    }
    void ReloadSettings()
    {
        audioSourceMove.volume = SettingsManager.SoundVolume;
        audioSourceJump.volume = SettingsManager.SoundVolume;
        audioSourceDash.volume = SettingsManager.SoundVolume;
        audioSourceAttack.volume = SettingsManager.SoundVolume;
        cameraSensX = SettingsManager.CameraSensX;
        cameraSensY = SettingsManager.CameraSensY;
    }
    void OnDisable()
    {
        actionMovement.action.Disable();
        actionLook.action.Disable();
        actionJump.action.Disable();
        actionJump.action.started -= Jump;
        actionDash.action.Disable();
        actionDash.action.started -= Dash;
        actionAttack.action.Disable();
        actionAttack.action.started -= Attack;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (canMove)
        {
            Vector2 rawLookInput = actionLook.action.ReadValue<Vector2>();
            Vector3 look = new Vector3(rawLookInput.y * -cameraSensY, 0, 0);
            cameraGameObject.transform.Rotate(look);
            rb.transform.Rotate(Vector3.up, rawLookInput.x * cameraSensX);

            Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);
            move *= moveSpeed * rb.linearDamping * Time.deltaTime;
            rb.AddRelativeForce(move);
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        RaycastHit[] hi = Physics.RaycastAll(transform.position, Vector3.down, 1.2f);
        bool real = true;
        foreach (RaycastHit bye in hi)
            if (bye.collider.gameObject.tag == "Ground") real = false;
        if (real) return;
        Vector3 jump = new Vector3(0, jumpForce * rb.linearDamping, 0);
        rb.AddRelativeForce(jump);
    }

    void Dash(InputAction.CallbackContext context)
    {
        if (!canDash) return;
        StartCoroutine(disableMovement());
        StartCoroutine(lerpCamera());
        Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);
        move *= dashSpeed * rb.linearDamping;
        rb.linearVelocity = Vector3.zero;
        rb.AddRelativeForce(move, ForceMode.Impulse);
    }


    void Attack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        StartCoroutine(disableAttack());
        animator.SetTrigger("attack");
        GameObject tmpAttackBox = Instantiate(attackBox, transform);
        Destroy(tmpAttackBox, attackLifetime);
    }

    IEnumerator disableAttack()
    {
        canAttack = false;
        yield return new WaitForSecondsRealtime(attackCooldown);
        canAttack = true;
    }
    IEnumerator disableMovement()
    {
        //Før/under dash
        canMove = false;
        canDash = false;
        rb.linearDamping /= 2;
        yield return new WaitForSecondsRealtime(dashDisablesMovementFor);
        //Efter dash
        canMove = true;
        rb.linearDamping *= 2;
        //Cooldown
        yield return new WaitForSecondsRealtime(dashCooldown);
        canDash = true;
    }

    IEnumerator lerpCamera()
    {
        motionBlurVolume.enabled = true;
        for (float t = 0; t <= 1; t += 10 * Time.deltaTime)
        {
            camera.fieldOfView = math.lerp(80f, 50f, t);
            yield return 1;
        }
        for (float t = 0; t <= 1; t += 2 * Time.deltaTime)
        {
            camera.fieldOfView = math.lerp(50f, 80f, t);
            yield return 1;
        }

        camera.fieldOfView = 80f;
        motionBlurVolume.enabled = false;
    }
}
