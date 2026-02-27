using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class PhysicsControl : MonoBehaviour
{

    public Rigidbody2D rb;
    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whattoDetect;
    public bool grounded;
    private RaycastHit2D hitInfoLeft;
    private RaycastHit2D hitInfoRight;

    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckpointUpper;
    [SerializeField] private Transform wallCheckpointLower;
    public bool wallDetected;
    private bool onLadder;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;


    private float gravityValue;
    void Start()
    {
        gravityValue = rb.gravityScale;
    }

    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckpointUpper.position, transform.right, wallRayDistance, whattoDetect);
        hitInfoWallLower = Physics2D.Raycast(wallCheckpointLower.position, transform.right, wallRayDistance, whattoDetect);
        Debug.DrawRay(wallCheckpointUpper.position, new Vector3(wallRayDistance, 0, 0), Color.red);
        Debug.DrawRay(wallCheckpointLower.position, new Vector3(wallRayDistance, 0, 0), Color.red);
        if (hitInfoWallUpper || hitInfoWallLower)
        {
            return true;
        }
        return false;
    }

    public void SetOnLadder(bool onLadder)
    {
        this.onLadder = onLadder;
        if (onLadder)
        {
            DisableGravity();
            ResetVelocity();
        }
        else
        {
            EnableGravity();
        }
    }

    private bool CheckGround()
    {
        hitInfoLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, groundRayDistance, whattoDetect);
        hitInfoRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, groundRayDistance, whattoDetect);

        Debug.DrawRay(leftGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.red);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.red);
        if (hitInfoLeft || hitInfoRight)
            return true;

        return false;
    }


    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        grounded = CheckGround();
        if (grounded || onLadder)
        {
            DisableGravity();
        }
        else
        {
            EnableGravity();
        }
        wallDetected = CheckWall();
    }
}
