using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerDashController : PlayerSubController
{
    
    public float dashTimer;
    public float dashSpeed;
    public float dashCooldown;

    public float dashDuration;
    private float startDashTime;

    public override void Initialize()
    {

    }

    public override void OnDisable()
    {
    }

    public override void OnEnable()
    {
        Dash();
    }

    public override void Update()
    {
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;

        if (Time.time - startDashTime > dashDuration)
        {
            if (Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer))
                playerController.SetController(playerController.groundController);
            else
                playerController.SetController(playerController.airController);
        }
    }

    public void Dash()
    {
            startDashTime = Time.time;
            dashTimer = dashCooldown;
            playerController.rb.velocity = new Vector2(playerController.transform.localScale.x * dashSpeed, playerController.rb.velocity.y);
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Dash:
                playerController.SetController(playerController.dashController);
                return;
            case PlayerInputType.Jump:
                playerController.SetController(playerController.airController);
                return;
        }
    }
}
