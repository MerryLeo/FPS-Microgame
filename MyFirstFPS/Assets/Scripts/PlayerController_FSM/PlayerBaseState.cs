using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController_FSM playerController);
    public abstract void Update(PlayerController_FSM playerController);
}
