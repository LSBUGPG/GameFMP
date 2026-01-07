using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    public Image[] hearts;
    private int lives = 2;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hurt");
            hearts[lives].enabled = false;
            source.Play();
            lives -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hurt");
            hearts[lives].enabled = false;
            source.Play();
            lives -= 1;
        }
    }
}
