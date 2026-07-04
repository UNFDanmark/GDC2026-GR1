using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference actionMovement;   // vector2
    public InputActionReference actionDash;       // button
    public InputActionReference actionJump;       // button
    public InputActionReference actionAttack;     // button
    public InputActionReference actionLook;       // vector2
    public float moveSpeed = 10f;
    public float cameraSensX = 0.5f;
    public float cameraSensY = 0.5f;
    public float jumpForce = 10f;
    public float dashSpeed = 10f;
    public float dashDisablesMovementFor = 0.5f;
    public float dashCooldown = 2f;
    public GameObject cameraGameObject;
    Rigidbody rb;
    Camera camera;
    bool canMove = true, canDash = true;

    public GameObject slashbox360;
    public float slashboxLifetime = 9999f;
    
    
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
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 rawLookInput = actionLook.action.ReadValue<Vector2>();
            Vector3 look = new Vector3(rawLookInput.y * cameraSensY, 0, 0);
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
        GameObject slashboxtemp = Instantiate(slashbox360, transform);
        Destroy(slashboxtemp, slashboxLifetime);
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
    }
}
