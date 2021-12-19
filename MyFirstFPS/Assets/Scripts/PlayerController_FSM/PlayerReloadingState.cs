using UnityEngine;

public class PlayerReloadingState : PlayerBaseState 
{
    public override void EnterState(PlayerController_FSM playerController) 
    {
        playerController.currentGun.Reload();
    }

    public override void Update(PlayerController_FSM playerController) 
    {
        // Continuous action
        playerController.ReloadingWalk();

        // Condition
        if (playerController.disableControl) 
        {
            playerController.TransitionToState(playerController.InactiveState);
        } 
        else 
        {
            bool grounded = playerController.CheckIfGrounded();
            bool walking = playerController.CheckIfPlayerIsWalking();
            bool running = walking && Input.GetButton("Sprint");

            if (!grounded && playerController.currentGun.Reloading) 
            {
                playerController.Fall();
            } 

            // Jumping State
            else if (!grounded && !playerController.currentGun.Reloading) 
            {
                playerController.TransitionToState(playerController.JumpingState);
            } 

            // Running State
            else if (running && !playerController.currentGun.Reloading) 
            {
                playerController.ResetFallSpeed();
                playerController.TransitionToState(playerController.RunningState);
            } 

            // Walking State
            else if (walking && !playerController.currentGun.Reloading) 
            {
                playerController.ResetFallSpeed();
                playerController.TransitionToState(playerController.WalkingState);
            } 

            // Idle State
            else if (!playerController.currentGun.Reloading) 
            {
                playerController.ResetFallSpeed();
                playerController.TransitionToState(playerController.IdleState);
            }
        }
    }
}
