using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class LaddersAbility : BaseAbility
{
    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    private bool touchingLadder;
    private string ladderParameterName = "Ladder";
    private int ladderParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        ladderParameterID = Animator.StringToHash(ladderParameterName);
    }

    public void TouchingLadder(bool touching)
    {
        touchingLadder = touching;
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
        if (!isPermitted)
        {
            return;
        }
        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedAnimator.enabled = true;
            return;
        }
        if (touchingLadder)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
        }
    }

    private void StopClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted)
        {
            return;
        }
        if (linkedStateMachine.currentState != PlayerStates.State.Ladders)
        {
            return;
        }
        linkedAnimator.enabled = false;
    }

    public override void EnterAbility()
    {
        linkedAnimator.enabled = true;
        linkedPhysics.SetOnLadder(true);
    }

    public override void ExitAbility()
    {
        Debug.Log("Exit ladder");
        linkedPhysics.SetOnLadder(false);
        linkedAnimator.enabled = true;
        UpdateAnimator();
    }

    public override void ProcessAbility()
    {
        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }

    public override void ProcessFixedAbility()
    {
        if (touchingLadder || linkedInput.verticalInput < 0)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput * climbSpeed);
        }
        else
        {
            linkedPhysics.rb.linearVelocity = Vector2.zero;
        }
    }


    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderParameterID, linkedStateMachine.currentState == PlayerStates.State.Ladders);
    }
}
