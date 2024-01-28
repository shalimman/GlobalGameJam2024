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
    public Animator anim;

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

    public float heehaDuration;
    public bool isLaughing;
    public float laughTimer;

    public int playerIndex;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isLaughing = false;
    }

    private void Update()
    {
        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", rb.velocity.y);

        rb.velocity = new Vector2(horizontal * groundSpeed, rb.velocity.y);

        if (laughTimer > 0)
            laughTimer -= Time.deltaTime;
        else
            isLaughing = false;

        if (!isLaughing)
        {
            if (horizontal != 0)
                sr.flipX = (horizontal < 0 ? true : false);
            else
                sr.flipX = false;
        }

        if (rb.velocity.y <= 0)
            rb.gravityScale = fallGravity;
        else
            rb.gravityScale = normalGravity;

        anim.SetBool("grounded", IsGrounded());
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
        if (IsGrounded())
        {
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }

    public void OnMove(Vector2 vec)
    {
        horizontal = vec.x;
    }

    public void OnHEEHA()
    {
        laughTimer = heehaDuration;
        isLaughing = true;
        sr.flipX = false;
        anim.SetTrigger("Laugh");
    }




}
