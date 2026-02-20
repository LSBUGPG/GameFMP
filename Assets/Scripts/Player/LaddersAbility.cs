using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class LaddersAbility : BaseAbility
{
    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float setMinLadderTime;
    private float minimumLadderTime;
    private bool climb;
    private bool tryingToClimb;
    private bool canGoOnLadder;


    private string ladderParameterName = "Ladder";
    private int ladderParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        ladderParameterID = Animator.StringToHash(ladderParameterName);
        minimumLadderTime = setMinLadderTime;
    }

    public void TouchingLadder(bool touching)
    {
        canGoOnLadder = touching;
        linkedPhysics.touchingLadder = touching;
    }

    private void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
        ladderActionRef.action.canceled += StopClimb;
    }

    private void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
        ladderActionRef.action.canceled -= StopClimb;
    }

    private void TryToClimb(InputAction.CallbackContext value)
    {
        tryingToClimb = true;
        TryToClimb();
    }

    private void TryToClimb()
    {
        if (!isPermitted)   
        {
            return;
        }
        linkedAnimator.enabled = true;
        if (linkedStateMachine.currentState==PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash || !canGoOnLadder)
        {
            return;
        }

        linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();
        climb = true;
        minimumLadderTime = setMinLadderTime;
        
    }
    private void StopClimb(InputAction.CallbackContext value)
    {
        tryingToClimb = false;
        if (!isPermitted)
        {
            return;
        }
        if (linkedStateMachine.currentState != PlayerStates.State.Ladders)
        {
            return;
        }
        linkedPhysics.ResetVelocity();
        linkedAnimator.enabled = false;
    }

    public override void EnterAbility()
    {
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        climb = false;
        linkedAnimator.enabled = true;
    }

    private void Update()
    {
        if (tryingToClimb /* || linkedStateMachine.currentState == PlayerStates.State.Jump*/)
        {
            TryToClimb();
        }
    }
    public override void ProcessAbility()
    {
        if (climb)
        {
            minimumLadderTime -= Time.deltaTime;
        }
        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.EnableGravity();
            return;
        }
        if (canGoOnLadder == false)
        {
            climb = false;
            if (linkedPhysics.grounded == false)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                //linkedPhysics.EnableGravity();
            }
        }
        if(linkedPhysics.grounded && minimumLadderTime <= 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            //linkedPhysics.EnableGravity();
        }
    }

   public override void ProcessFixedAbility()
    {
        if (climb)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput * climbSpeed);
        }
    }


    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderParameterID,linkedStateMachine.currentState == PlayerStates.State.Ladders);
    }
}
