using UnityEngine;

public class PlayerWalkingState : PlayerBaseState 
{
    const float _delay = 0.25f;
    float _time;

    public override void EnterState(PlayerController_FSM playerController) 
    {
        playerController.AudioSource.clip = playerController.AudioManagerScript.Walking;
        playerController.AudioSource.loop = true;
        _time = Time.time;
    }

    public override void Update(PlayerController_FSM playerController) 
    {
        // Continuous action
        playerController.Walk();

        if (Time.time >= _time + _delay && !playerController.AudioSource.isPlaying)
        {
            playerController.AudioSource.Play();
        } 

        // Condition
        if (playerController.disableControl) 
            playerController.TransitionToState(playerController.InactiveState);
         
        else 
        {
            // Shoot or Reload
            bool reload = Input.GetButton("Reload");
            bool fire = Input.GetButton("Fire1");
            if ((reload || fire && playerController.currentGun.AmmunitionInClip == 0) && playerController.currentGun.IsReloadingPossible)
                playerController.currentGun.Reload();
            else if (fire && playerController.currentGun.AmmunitionInClip > 0)
                playerController.currentGun.Shoot();

            // Jumping State
            if (!playerController.CheckIfGrounded()) 
            {
                playerController.TransitionToState(playerController.JumpingState);
                playerController.AudioSource.loop = false;
                playerController.AudioSource.Stop();
            }

            // Jumping State
            else if (Input.GetButton("Jump")) 
            {
                playerController.Jump();
                playerController.TransitionToState(playerController.JumpingState);
                playerController.AudioSource.loop = false;
                playerController.AudioSource.Stop();
            }

            // Reloading State
            //else if ((Input.GetButton("Reload") || (Input.GetButton("Fire1") && playerController.currentGun.AmmunitionInClip == 0)) && playerController.currentGun.IsReloadingPossible) 
            //{
            //    playerController.TransitionToState(playerController.ReloadingState);
            //    playerController.AudioSource.loop = false;
            //    playerController.AudioSource.Stop();
            //} 

            // Running State
            else if (Input.GetButton("Sprint")) 
            {
                playerController.TransitionToState(playerController.RunningState);
                playerController.AudioSource.loop = false;
                playerController.AudioSource.Stop();
            } 

            // Idle State
            else if (!playerController.CheckIfPlayerIsWalking()) 
            {
                playerController.TransitionToState(playerController.IdleState);
                playerController.AudioSource.loop = false;
                playerController.AudioSource.Stop();
            }
        }
        
    }
}
