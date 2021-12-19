using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController_FSM : MonoBehaviour 
{
    public PlayerBaseState CurrentState => _currentState;
    public float Speed => 10f;
    public float ReloadingWalkSpeed => 5f;
    public float FloatingWalkSpeed => 7f;
    public float SprintSpeed => 15f;
    public float Gravity => -19.62f;
    public float JumpHeight => 3f;
    public float GroundDst => 0.10f;
    public Vector3 PlayerVelocity => _playerVelocity;

    PlayerBaseState _currentState;
    CharacterController _controller;
    Vector3 _velocity, _playerVelocity;
    public bool disableControl;

    // States
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();
    public readonly PlayerRunningState RunningState = new PlayerRunningState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerInactiveState InactiveState = new PlayerInactiveState();

    // Audio
    public AudioSource AudioSource { get; private set; }
    public AudioManager AudioManagerScript { get; private set; }

    [SerializeField]
    Transform _groundCheck;

    [SerializeField]
    LayerMask _groundMask;

    [SerializeField]
    Transform cameraTransform;

    public IPlayerFirearm currentGun;

    void Awake() 
    {
        AudioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        AudioSource = GetComponent<AudioSource>();
        currentGun = (IPlayerFirearm)GetComponentInChildren(typeof(IPlayerFirearm));
        _controller = GetComponent<CharacterController>();
        _playerVelocity = Vector3.zero;
        disableControl = false;
        ResetFallSpeed();
    }

    void Start() 
    {
        TransitionToState(IdleState);
    }

    void Update() 
    {
        _currentState.Update(this);
    }

    public void Walk() 
    {    
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveVector = _playerVelocity = (horizontalInput * cameraTransform.right + verticalInput * new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z)) * Speed;
        _controller.Move(moveVector * Time.deltaTime);
    }

    public void ReloadingWalk() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveVector = _playerVelocity = (horizontalInput * cameraTransform.right + verticalInput * new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z)) * ReloadingWalkSpeed;
        _controller.Move(moveVector * Time.deltaTime);
    }

    public void Sprint() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveVector = _playerVelocity = (horizontalInput * cameraTransform.right + verticalInput * new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z)) * SprintSpeed;
        _controller.Move(moveVector * Time.deltaTime);
    }

    public void Jump()
    {
        _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
    }
        
    public void Fall() 
    {
        _velocity.y += Gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    public bool CheckIfPlayerIsWalking() 
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    public void ResetFallSpeed() 
    {
        _velocity.y = -2f;
    }

    public bool CheckIfGrounded() 
    {
        return Physics.CheckSphere(_groundCheck.position, GroundDst, _groundMask);
    }
    
    public void TransitionToState(PlayerBaseState state) 
    {
        _currentState = state;
        _currentState.EnterState(this);
    }
}
