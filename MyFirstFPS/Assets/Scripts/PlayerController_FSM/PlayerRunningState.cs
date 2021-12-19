using UnityEngine;

public class PlayerRunningState : PlayerBaseState 
{
    public override void EnterState(PlayerController_FSM playerController) 
    {
        playerController.AudioSource.clip = playerController.AudioManagerScript.Running;
        playerController.AudioSource.loop = true;
        playerController.AudioSource.Play();
    }

    public override void Update(PlayerController_FSM playerController) 
    {
        // Continuous action
        playerController.Sprint();

        // Condition
        if (playerController.disableControl)
        {
            playerController.AudioSource.Stop();
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
            if (!playerController.CheckIfGrounded())
            {
                playerController.AudioSource.Stop();
                playerController.TransitionToState(playerController.JumpingState);
            }
             
            // Transition to Jumping State with Jump action
            else if (Input.GetButton("Jump")) 
            {
                playerController.AudioSource.Stop();
                playerController.Jump();
                playerController.TransitionToState(playerController.JumpingState);
            }

            // Transition to Running State
            else if (!Input.GetButton("Sprint") && playerController.CheckIfPlayerIsWalking())
            {
                playerController.AudioSource.Stop();
                playerController.TransitionToState(playerController.WalkingState);
            }

            // Transition to Idle State
            else if (!Input.GetButton("Sprint") && !playerController.CheckIfPlayerIsWalking())
            {
                playerController.AudioSource.Stop();
                playerController.TransitionToState(playerController.IdleState);
            }
        }
    }
}
