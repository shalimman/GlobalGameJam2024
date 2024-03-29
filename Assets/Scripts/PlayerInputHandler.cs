using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var movers = FindObjectsOfType<PlayerController>();
        var index = playerInput.playerIndex;
        playerController = movers.FirstOrDefault(m => m.GetPlayerIndex() == index);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && playerController != null)
        {
            playerController.OnJump();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(playerController != null)
            playerController.OnMove(context.ReadValue<Vector2>());
    }

    public void Laugh(InputAction.CallbackContext context)
    {
        if (context.started && playerController != null)
        {
            playerController.OnHEEHA();
        }
    }

}