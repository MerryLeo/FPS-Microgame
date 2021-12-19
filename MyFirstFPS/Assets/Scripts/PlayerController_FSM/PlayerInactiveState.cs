

public class PlayerInactiveState : PlayerBaseState 
{
    public override void EnterState(PlayerController_FSM playerController) 
    {
        
    }

    public override void Update(PlayerController_FSM playerController) 
    {
        if (!playerController.disableControl) 
        {
            playerController.TransitionToState(playerController.IdleState);
        }
    }
}
