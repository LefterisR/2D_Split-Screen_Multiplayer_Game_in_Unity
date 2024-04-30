using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveTest : MonoBehaviour
{
    //private PlayerInputActions playerInputActions;

    
    private InputActionAsset inputAsset;
    private InputActionMap player;

    private InputAction move;


    [SerializeField] private float moveSpeed = 10f;
    private Vector2 moveInput;
    [SerializeField] private string actionMap;

    Rigidbody2D rb;

    private void Awake()
    {
        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap(actionMap);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        move = player.FindAction("Move");
        move.Enable();
        
        player.FindAction("Jump").performed += DoJump; //Subscribe Function
        player.FindAction("Jump").Enable();

        

    }

    private void DoJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        rb.velocity = new Vector2(0, 10f);
    }

    private void OnDisable()
    {
        move.Disable();
        player.FindAction("Jump").Disable();
    }
    private void Start()
    {
       
    }
    private void FixedUpdate()
    {
        moveInput = move.ReadValue<Vector2>();

        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

    }
}
