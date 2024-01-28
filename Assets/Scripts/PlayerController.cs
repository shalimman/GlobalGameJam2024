using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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

    [Header("Player Components")]
    public SpriteRenderer sr;
    public Rigidbody2D rb;

    [SerializeField]
    public Transform groundCheck;
    [SerializeField]
    public Transform wallCheck;
    [SerializeField]
    public LayerMask groundLayer;


    [Header("Player Stats")]
    public float horizontal;
    public float groundSpeed;
    public float airSpeed;
    public float jumpingPower;
    public float normalGravity;
    public float fallGravity;

    public int playerIndex;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontal * groundSpeed, rb.velocity.y);

        if (horizontal != 0)
            transform.localScale = new Vector3((horizontal < 0 ? -1.0f : 1.0f), 1.0f, 1.0f);

        if (rb.velocity.y <= 0)
            rb.gravityScale = fallGravity;
        else
            rb.gravityScale = normalGravity;
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void OnJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
    }

    public void OnMove(Vector2 vec)
    {
        horizontal = vec.x;
    }

    

}
