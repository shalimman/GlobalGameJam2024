using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public enum PlayerInputType
{
    Jump,
    Dash,
    Attack,
}

[System.Serializable]
public abstract class PlayerSubController
{
    [HideInInspector]
    public PlayerController playerController;

    /// <summary>
    /// Called when this subcontroller is getting initialized when the player starts. You can assume the player controller is initialized.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Called when this subcontroller becomes the new subcontroller of the character
    /// </summary>
    public abstract void OnEnable();

    /// <summary>
    /// Called when this subcontroller stops being the new subcontroller of the character
    /// </summary>
    public abstract void OnDisable();

    /// <summary>
    /// Called when the subcontroller is the current subcontroller of the character
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Called when the subcontroller receives input from the player controller. Only called when it is the current subcontroller.
    /// </summary>
    /// <param name="type"></param>
    public abstract void RecieveInput(PlayerInputType type);
}

public class PlayerController : MonoBehaviour
{
    [Header("Player Subcontrollers")]
    public PlayerGroundController groundController;
    public PlayerAirController airController;
    public PlayerDashController dashController;
    public PlayerWallController wallController;

    [Header("Player Components")]
    [SerializeField]
    public SpriteRenderer sr;
    public Rigidbody2D rb;

    [SerializeField]
    public Transform groundCheck;
    [SerializeField]
    public Transform wallCheck;
    [SerializeField]
    public LayerMask groundLayer;
    [SerializeField]
    public LayerMask powerupLayer;
    [SerializeField]
    public LayerMask wallLayer;

    [Header("Player Stats")]
    [SerializeField]
    public float horizontal;
    [SerializeField]
    public float speed;
    

    public Vector2 aimDirection;

    [SerializeField]
    public PlayerSubController currentController { get; private set; }

    private void Awake()
    {
        groundController.playerController = this;
        groundController.Initialize();
        airController.playerController = this;
        airController.Initialize();
        dashController.playerController = this;
        dashController.Initialize();
        wallController.playerController = this;
        wallController.Initialize();

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        SetController(groundController);
    }

    void FixedUpdate()
    {
        currentController.Update();
    }

    public void SetController(PlayerSubController newController)
    {
        if (currentController != null)
            currentController.OnDisable();

        currentController = newController;
        currentController.OnEnable();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentController.RecieveInput(PlayerInputType.Jump);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;

        aimDirection = context.ReadValue<Vector2>();
        aimDirection.x = Mathf.Round(aimDirection.x);
        aimDirection.y = (IsGrounded() ? Mathf.Clamp(Mathf.Round(aimDirection.y), 0, 1) : Mathf.Round(aimDirection.y));
        aimDirection.Normalize();
    }

}
