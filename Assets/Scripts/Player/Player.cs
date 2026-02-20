using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public StateMachine stateMachine;
    public Animator anim;
    public PhysicsControl physicsControl;

    private BaseAbility[] playerAbilities;
    public bool facingRight = true;

    private void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities=GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
    }


    private void Update()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
    }
    private void FixedUpdate()
    {
        foreach(BaseAbility ability in playerAbilities )
        {
            if(ability.thisAbilityState ==stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }


    public void Flip()
    {
        if(facingRight== true && gatherInput.horizontalInput < 0 )
        {
            transform.Rotate(0, 180, 0);
            //var currentScale = transform.localScale;
            //currentScale.x = Mathf.Abs(currentScale.x) *-1.0f;
            //transform.localScale = currentScale;
            facingRight = !facingRight;
        }
        else if (facingRight== false && gatherInput.horizontalInput > 0 )
        {
            //var currentScale = transform.localScale;
            //currentScale.x = Mathf.Abs(currentScale.x);
            //transform.localScale = currentScale;
            transform.Rotate(0,180,0);
            facingRight= !facingRight;
        }
    }
    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}
