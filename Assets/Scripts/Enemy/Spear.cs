using System.Collections;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float moveSpeed;
    public float startDelay;
    public float endDelay;
    public Transform spearObject;

    private float direction = -1f;

    private bool canMove = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == false) { return; }
        transform.Translate(0, moveSpeed * Time.deltaTime * direction, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("StartStop") || collision.CompareTag("EndStop"))
        {
            direction = -direction;
            canMove = false;
            if (collision.CompareTag("StartStop"))
            {
                StartCoroutine(StartDelay());
            }
            if (collision.CompareTag("EndStop"))
            {
                StartCoroutine(EndDelay());
            }
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        canMove = true;
    }
    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(endDelay);
        canMove = true;
    }
}
