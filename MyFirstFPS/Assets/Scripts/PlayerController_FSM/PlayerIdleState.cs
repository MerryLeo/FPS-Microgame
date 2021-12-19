using UnityEngine;

public class PlayerIdleState : PlayerBaseState 
{
    public override void EnterState(PlayerController_FSM playerController) 
    {
        
    }

    public override void Update(PlayerController_FSM playerController) 
    {
        // Transition to inactive State
        if (playerController.disableControl) 
        {
            playerController.TransitionToState(playerController.InactiveState);
        }
        else 
        {

            // Shoot or Reload
            bool reload = Input.GetButton("Reload");
            bool fire = Input.GetButton("Fire1");
            if ((reload || fire && playerController.currentGun.AmmunitionInClip == 0) && playerController.currentGun.IsReloadingPossible)
                playerController.currentGun.Reload();
            else if (fire && playerController.currentGun.AmmunitionInClip > 0)
                playerController.currentGun.Shoot();

            // Transition to Jumping State
            bool grounded = playerController.CheckIfGrounded();
            if (!grounded) 
            {
                playerController.TransitionToState(playerController.JumpingState);
            }

            // Transition to Jumping State with Jump action
            else if (Input.GetButton("Jump")) 
            {
                playerController.Jump();
                playerController.TransitionToState(playerController.JumpingState);
            }

            // Transition to Reloading State
            //else if ((Input.GetButton("Reload") || (Input.GetButton("Fire1") && playerController.currentGun.AmmunitionInClip == 0)) && playerController.currentGun.IsReloadingPossible) 
            //{
            //    playerController.TransitionToState(playerController.ReloadingState);
            //} 

            // Transition to Running State
            else if (Input.GetButton("Sprint")) 
            {
                playerController.TransitionToState(playerController.RunningState);
            }

            // Transition to Walking State
            else if (playerController.CheckIfPlayerIsWalking()) 
            {
                playerController.TransitionToState(playerController.WalkingState);
            }
        }
    }
}
