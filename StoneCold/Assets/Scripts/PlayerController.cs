using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;

    [Header("Player Sprite")]
    [SerializeField]
    private GameObject player;

    private Rigidbody2D playerRB;

    private Vector2 playerMove;
    private Vector2 playerRotation;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    public void RotateToTarget(Vector2 targetDirection)
    {   
        if(targetDirection == new Vector2(0,0)) return;
        // Calculate the angle in radians between the current object's position and the target direction
        float angle = Mathf.Atan2(targetDirection.x, targetDirection.y);

        // Convert the angle to degrees
        float angleDegrees = -angle * Mathf.Rad2Deg;

        // Create a rotation quaternion based on the calculated angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, angleDegrees);

        // Apply the rotation to the object
        player.transform.rotation = targetRotation;
    }

    private void FixedUpdate()
    {
        playerMove = playerControls.Player.Move.ReadValue<Vector2>() * moveSpeed;
        playerRotation = playerControls.Player.LookShoot.ReadValue<Vector2>();

        //Debug.Log("playerRotationVal: " + playerRotation);

        playerRB.position = playerRB.position + new Vector2(playerMove.x, playerMove.y);
        RotateToTarget(playerRotation);
    }
}
