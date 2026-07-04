using System.Collections;
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
    public GameObject camera;
    Rigidbody rb;
    bool canMove = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        actionMovement.action.Enable();
        actionLook.action.Enable();
        actionJump.action.Enable();
        actionJump.action.started += Jump;
        actionDash.action.Enable();
        actionDash.action.started += Dash;
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 rawLookInput = actionLook.action.ReadValue<Vector2>();
            Vector3 look = new Vector3(rawLookInput.y * cameraSensY, 0, 0);
            camera.transform.Rotate(look);
            rb.transform.Rotate(Vector3.up, rawLookInput.x * cameraSensX);

            Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);
            move *= moveSpeed * rb.linearDamping;
            rb.AddRelativeForce(move);
        }
    }

    void Jump(InputAction.CallbackContext input)
    {
        RaycastHit[] hi = Physics.RaycastAll(transform.position, Vector3.down, 1.2f);
        bool real = true;
        foreach (RaycastHit bye in hi)
            if (bye.collider.gameObject.tag == "Ground") real = false;
        if (real) return;
        Vector3 jump = new Vector3(0, jumpForce * rb.linearDamping, 0);
        rb.AddRelativeForce(jump);
    }

    void Dash(InputAction.CallbackContext input)
    {
        Vector2 rawMoveInput = actionMovement.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(rawMoveInput.x, 0, rawMoveInput.y);
        move *= dashSpeed * rb.linearDamping;
        rb.linearVelocity = Vector3.zero;
        rb.AddRelativeForce(move, ForceMode.Impulse);
    }
}
