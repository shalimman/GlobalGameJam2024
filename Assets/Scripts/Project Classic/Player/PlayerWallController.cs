using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWallController : PlayerSubController
{
    Collider2D wall;

    public float wallSlideAcceleration;
    public float jumpOffWallVelocity;

    float currentSlideSpeed;

    public override void Initialize()
    {
        
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        wall = Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer);

        currentSlideSpeed = 0;
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Jump:
                PerformWallJump();
                break;
        }

    }

    public override void Update()
    {
        currentSlideSpeed += wallSlideAcceleration * Time.deltaTime;
        playerController.rb.velocity = new Vector2(0, -currentSlideSpeed);

        bool isOnGround = Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer);

        if (isOnGround)
        {
            playerController.airController.ResetJumps();
            playerController.SetController(playerController.groundController);
        }
        else
        {
            bool isOnWall = Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer);
            if (!isOnWall)
            {
                playerController.SetController(playerController.airController);
            }
        }
    }

    void PerformWallJump()
    {
        bool isWallOnRight = wall.transform.position.x - playerController.transform.position.x > 0;
        float horizontalJumpVelocity = isWallOnRight ? -jumpOffWallVelocity : jumpOffWallVelocity;
        playerController.rb.velocity = new Vector2(horizontalJumpVelocity, 0);
        playerController.airController.ResetJumps();
        playerController.horizontal = (isWallOnRight ? -1f : 1f);
        playerController.SetController(playerController.airController);
    }
}
