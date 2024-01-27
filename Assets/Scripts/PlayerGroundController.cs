using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


[System.Serializable]
public class PlayerGroundController : PlayerSubController
{
    public float groundSpeed;

    public override void Initialize()
    {

    }

    public override void OnDisable()
    {

    }

    public override void OnEnable()
    {
        playerController.airController.ResetJumps();
        playerController.rb.gravityScale = 10f;

    }

    public override void Update()
    {
        playerController.rb.velocity = new Vector2(playerController.horizontal * groundSpeed, playerController.rb.velocity.y);

        if (playerController.horizontal != 0)
            playerController.transform.localScale = new Vector3((playerController.horizontal < 0 ? -1.0f : 1.0f), 1.0f, 1.0f);
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
