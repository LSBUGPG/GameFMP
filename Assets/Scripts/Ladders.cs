using UnityEngine;

public class Ladders : MonoBehaviour
{
    private LaddersAbility laddersAbility;

    private void SetTouchingLadder(bool touching)
    {
        if (laddersAbility != null)
        {
            if (laddersAbility.isPermitted)
            {
                laddersAbility.TouchingLadder(touching);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        laddersAbility = collision.GetComponent<LaddersAbility>();
        SetTouchingLadder(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        SetTouchingLadder(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SetTouchingLadder(false);
        laddersAbility = null;
    }
}
