using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour 
{
    [SerializeField] private int speed = 10;
    [SerializeField] private bool usePhysics = true;

    private Rigidbody _rb;
    private Camera _mainCamera;
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controls.Enable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        _controls.Disable();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (usePhysics)
        {
            return;
        }

        if (_controls.Player.Move.IsPressed())
        {
            Vector2 input = _controls.Player.Move.ReadValue<Vector2>();
            Vector3 target = HandleInput(input);
            Move(target);
        }
    }

    private void FixedUpdate()
    {
        if (!usePhysics)
        {
            return;
        }

        if (_controls.Player.Move.IsPressed())
        {
            Vector2 input = _controls.Player.Move.ReadValue<Vector2>();
            Vector3 target = HandleInput(input);
            MovePhysics(target);
        }
    }

    private Vector3 HandleInput(Vector2 input)
    {
        Vector3 forward = _mainCamera.transform.forward;
        Vector3 right = _mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = right * input.x + forward * input.y;

        return transform.position + direction * speed * Time.deltaTime;
    }

    private void Move(Vector3 target)
    {
        transform.position = target;
    }

    private void MovePhysics(Vector3 target)
    {
        _rb.MovePosition(target);
    }

    private void Jump() 
    {
        Debug.Log("Jump!");
        _rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
    }
}
