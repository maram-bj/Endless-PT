using UnityEngine;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float horizontalSpeed = 5f;
    public float boundaryLimit = 2f;

    private bool isGrounded;
    private Rigidbody rb;
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>(); 
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        float horizontalInput = Input.GetAxis("Horizontal");
        float newPosX = transform.position.x + horizontalInput * horizontalSpeed * Time.deltaTime;

        newPosX = Mathf.Clamp(newPosX, -boundaryLimit, boundaryLimit);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player landed on the ground.");
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit an obstacle.");
            playerHealth.TakeDamage(1); 
            Debug.Log("Player health: " + playerHealth.currentHealth);
        }
    }
}
