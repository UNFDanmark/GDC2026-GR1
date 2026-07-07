using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    public float audioCooldown = 0.5f;
    float audioCooldownLeft;
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
    bool invulnerable = false;
    
    [Header("Slide")]
    public InputActionReference actionSlide;
    public float slideSpeed, slideThreshold, slideCooldown, slideHeight;
    bool dashing;
    
    [Header("Camera")]
    public InputActionReference actionLook;       // vector2
    public GameObject cameraGameObject;
    float cameraSensX = 0.5f;
    float cameraSensY = 0.5f;
    Camera camera;
    
    [Header("Attack")]
    public GameObject attackBox;
    public float attackLifetime = 0.2f;
    public AudioSource audioSourceAttack;
    public AudioSource audioSourceDeath;

    [Header("Deathscreen")]
    public Canvas Deathscreen; 

   
    
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
        actionSlide.action.Enable();
        actionSlide.action.started += Slide;
        actionSlide.action.canceled += UnSlide;
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
        actionSlide.action.Disable();
        actionSlide.action.started -= Slide;
        actionSlide.action.canceled -= UnSlide;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        Vector2 rawLookInput = actionLook.action.ReadValue<Vector2>();
        Vector3 look = new Vector3(rawLookInput.y * -cameraSensY, 0, 0);
        cameraGameObject.transform.Rotate(look);
        rb.transform.Rotate(Vector3.up, rawLookInput.x * cameraSensX);
        if (canMove) {
            Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);
            move *= moveSpeed * rb.linearDamping * Time.deltaTime;
            rb.AddRelativeForce(move);
        }

        if (audioCooldownLeft <= 0 && rb.linearVelocity.magnitude > 4)
        {
            audioSourceMove.Play();
            audioCooldownLeft = audioCooldown;
        }   
        audioCooldownLeft -= Time.deltaTime;
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
        audioSourceJump.Play();
    }

    void Dash(InputAction.CallbackContext context)
    {
        if (!canDash) return;
        Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
        if (rawMoveInput == Vector2.zero) return;
        StartCoroutine(disableMovement(dashDisablesMovementFor, dashCooldown));
        StartCoroutine(lerpCamera());
        
        Vector3 move = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);
        move *= dashSpeed * rb.linearDamping;
        rb.linearVelocity = Vector3.zero;
        rb.AddRelativeForce(move, ForceMode.Impulse);
        
        animator.SetTrigger("attack");
        GameObject tmpAttackBox = Instantiate(attackBox, transform);
        audioSourceAttack.Play();
        Destroy(tmpAttackBox, attackLifetime);
        audioSourceDash.Play();
    }

    void Slide(InputAction.CallbackContext context)
    {
        if (!canDash) return;
        if (rb.linearVelocity.magnitude < slideThreshold) return;
        dashing = true;
        canMove = false;
        canDash = false;
        
        animator.SetTrigger("crouching");
        Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(rawMoveInput.x, slideHeight, rawMoveInput.y);
        move *= slideSpeed * rb.linearDamping;
        rb.AddRelativeForce(move, ForceMode.Impulse);
    }

    void UnSlide(InputAction.CallbackContext context)
    {
        if (!dashing) return;
        dashing = false;
        canMove = true;
        StartCoroutine(disableMovement(0, slideCooldown));
        animator.SetTrigger("uncrouching");
    }
    
    IEnumerator disableMovement(float disableTime, float cooldown)
    {
        //Før/under dash
        canMove = false;
        canDash = false;
        rb.linearDamping /= 2;
        invulnerable = true;
        yield return new WaitForSecondsRealtime(disableTime);
        //Efter dash
        invulnerable = false;
        canMove = true;
        rb.linearDamping *= 2;
        //Cooldown
        yield return new WaitForSecondsRealtime(cooldown);
        canDash = true;
    }

    IEnumerator lerpCamera()
    {
        motionBlurVolume.enabled = true;
        for (float t = 0; t <= 1; t += 10 * Time.deltaTime)
        {
            camera.fieldOfView = math.lerp(80f, 50f, t);
            //Time.timeScale = math.lerp(1f, 0.5f, t);
            yield return 1;
        }
        for (float t = 0; t <= 1; t += 2 * Time.deltaTime)
        {
            camera.fieldOfView = math.lerp(50f, 80f, t);
            //Time.timeScale = math.lerp(0.5f, 1f, t);
            yield return 1;
        }

        camera.fieldOfView = 80f;
        motionBlurVolume.enabled = false;
    }
    

    void OnCollisionEnter(Collision other)
    {
        if (invulnerable) return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            canDash = false;
            canMove = false;
            Deathscreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
            SettingsApplier.canPause = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            audioSourceDeath.Play();
        }
    }
}
