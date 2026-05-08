using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    [Header("[== MOVEMENT SETTINGS ==]")]
    [SerializeField] private float directionSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("[== REFERENCES ==]")]
    [SerializeField] private Transform playerObj;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private Keyboard keyboard;
    private KeyControl left, right, forward, backward;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        // set keyboard controls
        keyboard = Keyboard.current;
        left = keyboard.aKey;
        right = keyboard.dKey;
        forward = keyboard.wKey;
        backward = keyboard.sKey;

        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        HandleInput();
    }

    void HandleInput()
    {
        float x = 0f, z = 0f;

        // movement
        if (left.isPressed) x -= 1f;
        if (right.isPressed) x += 1f;
        if (forward.isPressed) z += 1f;
        if (backward.isPressed) z -= 1f;

        Transform cam = Camera.main.transform;

        Vector3 forwardAxis = cam.forward;
        Vector3 rightAxis = cam.right;

        forwardAxis.y = 0f;
        rightAxis.y = 0f;

        forwardAxis.Normalize();
        rightAxis.Normalize();

        moveDirection = forwardAxis * z + rightAxis * x;
        moveDirection.Normalize();
    }

    void RotatePlayer()
    {
        if (moveDirection == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        playerObj.rotation = Quaternion.Slerp(
            playerObj.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void MovePlayer()
    {
        rb.MovePosition(rb.position + moveDirection * directionSpeed * Time.deltaTime);
    }
}