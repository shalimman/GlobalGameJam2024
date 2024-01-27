using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class PlayerAirController : PlayerSubController
{
    public float airSpeed;

    public float jumpingPower;
    public int maxExtraJumps;
    public int jumpsRemaining;

    public override void Initialize()
    {
        jumpsRemaining = maxExtraJumps;
    }

    public override void OnDisable()
    {
    }

    public override void OnEnable()
    {
        if (Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer))
            Jump();
        else
            ExtraJump();
    }

    public override void Update()
    {
        playerController.rb.velocity = new Vector2(playerController.horizontal * airSpeed, playerController.rb.velocity.y);

        if (playerController.horizontal != 0)
            playerController.transform.localScale = new Vector3((playerController.horizontal < 0 ? -1.0f : 1.0f), 1.0f, 1.0f);

        if (Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer))
        {
            ResetJumps();
            playerController.SetController(playerController.groundController);
        }

        if(playerController.rb.velocity.y < 0)
        {
            playerController.rb.gravityScale = 0.2f;
        }

        //if (Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer))
        //{
        //    playerController.SetController(playerController.wallController);
        //}
    }

    public void Jump()
    {
        if (jumpsRemaining > 0)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpingPower);
            jumpsRemaining--;
        }
        else
            playerController.SetController(playerController.groundController);

    }

    public void ExtraJump()
    {
        if (jumpsRemaining > 0)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpingPower);
            jumpsRemaining--;
        }
    }

    public void ResetJumps()
    {
        jumpsRemaining = maxExtraJumps;
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Dash:
                playerController.SetController(playerController.dashController);
                return;
            case PlayerInputType.Jump:
                ExtraJump();
                return;
        }
    }

    
}
