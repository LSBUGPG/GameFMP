using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Turret: MonoBehaviour
{

    public float firePerSecond;
    private float count = 0;
    public GameObject bullet;
    public Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime * firePerSecond;
        if (count > 1f)
        {
           var clone = Instantiate(bullet, transform.position,Quaternion.identity) as GameObject;
            clone.GetComponent<Bullet>().direction = direction;
            count = 0;

        }

    }
}
