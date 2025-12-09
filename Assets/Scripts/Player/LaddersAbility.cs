using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaddersAbility : BaseAbility
{
    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float setMinLadderTime;
    private float minimumLadderTime;
    private bool climb;
    public bool canGoOnLadder;


    protected override void Initialization()
    {
        base.Initialization();
        minimumLadderTime = setMinLadderTime;
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
        if(linkedStateMachine.currentState==PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash || !canGoOnLadder)
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
        if (!isPermitted)
        {
            return;
        }
        if (linkedStateMachine.currentState != PlayerStates.State.Ladders)
        {
            return;
        }
        linkedPhysics.ResetVelocity();
    }

    public override void ProcessAbility()
    {
        if (climb)
        {
            minimumLadderTime -= Time.deltaTime;
        }
    }

   public override void ProcessFixedAbility()
    {
        if (climb)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput * climbSpeed);
        }
    }
}
