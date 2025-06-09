using UnityEngine;

public class Obstacle2 : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2f;
    Rigidbody2D rb;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;
    public GameObject explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        // Add a Rigidbody2D component to the GameObject if it doesn't already have one
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed / randomSize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }
}
