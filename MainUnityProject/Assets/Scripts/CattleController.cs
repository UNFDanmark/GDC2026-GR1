using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CattleController : MonoBehaviour
{
    Rigidbody rb;
    [Header("Movement")]
    public InputAction actionMovement;   // float
    public InputAction actionRotate;   // float
    public float moveSpeed = 10f;
    public float cosmeticRotateFactor;
    public float rotateSpeed;
    float velocity = 0;
    float targetVelocity = 0;
    float targetZRotation;
    void Start()
    {
        actionMovement.Enable();
        actionRotate.Enable();
        rb = GetComponent<Rigidbody>();
    }

    void OnDisable()
    {
        actionRotate.Disable();
        actionMovement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float rawMoveInput = actionMovement.ReadValue<float>();
        targetVelocity = rawMoveInput * moveSpeed;
        velocity += (targetVelocity - velocity) * Time.deltaTime;
        rb.linearVelocity = velocity * transform.forward;

        float rawRotateInput = actionRotate.ReadValue<float>();
        transform.Rotate(0,rawRotateInput * rotateSpeed * Time.deltaTime,0, Space.World);
        //targetZRotation -= (rawRotateInput * cosmeticRotateFactor + targetZRotation) * 3 * Time.deltaTime;
    }
}
