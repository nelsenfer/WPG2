using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float moveInput = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Menggerakkan player hanya pada sumbu X (kiri/kanan)
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
}