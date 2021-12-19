using UnityEngine;

public class PlayerJumpingState : PlayerBaseState 
{
    const float _checkIfGroundedCoolDown = 0.2f;
    float _checkIfGroundedTimeStamp;
    public override void EnterState(PlayerController_FSM playerController) 
    {
        // Jumping sound
        playerController.AudioSource.PlayOneShot(playerController.AudioManagerScript.Jumping);

        _checkIfGroundedTimeStamp = Time.time + _checkIfGroundedCoolDown;
    }

    public override void Update(PlayerController_FSM playerController) 
    {
        // Continuous actions
        playerController.Walk();
        playerController.Fall();

        // Conditions
        if (playerController.disableControl) 
            playerController.TransitionToState(playerController.InactiveState);
         
        else 
        {
            bool grounded = playerController.CheckIfGrounded();
            bool walking = playerController.CheckIfPlayerIsWalking();
            bool running = Input.GetButton("Sprint") && walking;

            // Shoot or Reload
            bool reload = Input.GetButton("Reload");
            bool fire = Input.GetButton("Fire1");
            if ((reload || fire && playerController.currentGun.AmmunitionInClip == 0) && playerController.currentGun.IsReloadingPossible)
                playerController.currentGun.Reload();
            else if (fire && playerController.currentGun.AmmunitionInClip > 0)
                playerController.currentGun.Shoot();

            // Running State
            if (grounded && running && Time.time >= _checkIfGroundedTimeStamp) 
            {
                playerController.AudioSource.PlayOneShot(playerController.AudioManagerScript.Landing);
                playerController.ResetFallSpeed();
                playerController.TransitionToState(playerController.RunningState);
            }

            // Walking State
            else if (grounded && walking && Time.time >= _checkIfGroundedTimeStamp) 
            {
                playerController.AudioSource.PlayOneShot(playerController.AudioManagerScript.Landing);
                playerController.ResetFallSpeed();
                playerController.TransitionToState(playerController.WalkingState);
            } 

            // Idle State
            else if (grounded && Time.time >= _checkIfGroundedTimeStamp) 
            {
                playerController.AudioSource.PlayOneShot(playerController.AudioManagerScript.Landing);
                playerController.ResetFallSpeed();
                playerController.TransitionToState(playerController.IdleState);
            }
        }
    }
}
