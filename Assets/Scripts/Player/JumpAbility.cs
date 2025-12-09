using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAbility : BaseAbility
{
    public InputActionReference jumpActionRef;

    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;
    [SerializeField] private float mininumAirTime;
    private float startmininumAirTime;

    private string jumpAnimParameterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int jumpParameterID;
    private int ySpeedParameterID;


    protected override void Initialization()
    {
        base.Initialization();
        startmininumAirTime = mininumAirTime;
        jumpParameterID = Animator.StringToHash(jumpAnimParameterName);
        ySpeedParameterID = Animator.StringToHash(ySpeedAnimParameterName);
    }
    private void OnEnable()
    {
        jumpActionRef.action.performed += TryToJump;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= TryToJump;
        jumpActionRef.action.canceled -= StopJump;

    }
    public override void ProcessAbility()
    {
        mininumAirTime -= Time.deltaTime;
        if(linkedPhysics.grounded && mininumAirTime< 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if(!linkedPhysics.grounded && linkedPhysics.wallDetected)
        {
            if(linkedPhysics.rb.linearVelocityY < 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            }
        }
    }
    public override void ProcessFixedAbility()
    {
        if (!linkedPhysics.grounded)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, linkedPhysics.rb.linearVelocityY);
        }
    }
    private void TryToJump(InputAction.CallbackContext value)
    {
        if (isPermitted == false)
        {
            return;
        }

        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            // linkedPhysics.EnableGravity();
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, 0 );
            mininumAirTime = startmininumAirTime;
            return;
        }
            
        Debug.Log(linkedPhysics.grounded);
        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed * linkedInput.horizontalInput, jumpForce);
            mininumAirTime = startmininumAirTime;
        }
    }

    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("STOPJUMP");
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParameterID, linkedStateMachine.currentState == PlayerStates.State.Jump || linkedStateMachine.currentState==PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(ySpeedParameterID, linkedPhysics.rb.linearVelocityY);
    }

}
