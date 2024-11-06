using UnityEngine;

public class LaserBulletShooter : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform shootPoint; // The point from where the bullet is shot
    public float shootInterval = 2f; // Interval between each shot
    public float bulletSpeed = 5f; // Speed of the bullet
    public float bulletLifetime = 5f; // How long before the bullet is destroyed

    [Header("Direction Settings")]
    public bool shootHorizontally = true; // If true, bullets shoot horizontally. If false, bullets shoot vertically.

    void Start()
    {
        // Repeatedly call ShootBullet at regular intervals
        InvokeRepeating(nameof(ShootBullet), 0f, shootInterval);
    }

    void ShootBullet()
    {
        // Instantiate the bullet at the shoot point
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        // Set the bullet to be destroyed after a certain time to avoid memory leaks
        Destroy(bullet, bulletLifetime);

        // Move the bullet
        BulletMover bulletMover = bullet.AddComponent<BulletMover>();
        bulletMover.Initialize(bulletSpeed, shootHorizontally);
    }
}

public class BulletMover : MonoBehaviour
{
    private float speed;
    private bool moveHorizontally;

    // Initialize method to set the bullet's speed and direction
    public void Initialize(float bulletSpeed, bool shootHorizontally)
    {
        speed = bulletSpeed;
        moveHorizontally = shootHorizontally;
    }

    void Update()
    {
        // Move the bullet in the specified direction
        if (moveHorizontally)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle player death
            collision.GetComponent<SimplePlayerMovement>().Die();
        }
        else if (collision.CompareTag("Obstacle"))
        {
            // Destroy the bullet if it hits an obstacle
            Destroy(gameObject);
        }
    }
}
