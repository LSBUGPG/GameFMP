using UnityEngine;

public class Ladder2 : MonoBehaviour
{
    private LadderAbility2 LadderAbility2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LadderAbility2 = collision.GetComponent<LadderAbility2>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LadderAbility2 != null)
        {
            if (LadderAbility2.isPermitted)
            {
                LadderAbility2.canGoOnLadder = true;
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LadderAbility2 != null)
        {
            if (LadderAbility2.isPermitted)
            {
                LadderAbility2.canGoOnLadder = false;
            }
        }
    }
}
