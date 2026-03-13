using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    // Tambahkan referensi untuk membaca script inventory
    private Inventory inventoryPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Mengambil script Inventory yang menempel di Player yang sama
        inventoryPlayer = GetComponent<Inventory>();
    }

    void Update()
    {
        // CEK INVENTORY: Jika terbuka, hentikan pergerakan dan abaikan input W/A/S/D
        if (inventoryPlayer.isOpen == true)
        {
            rb.linearVelocity = Vector2.zero; // Rem mendadak agar tidak jalan terus/meluncur
            return; // Hentikan fungsi Update di baris ini
        }

        // --- Logika pergerakan di bawah ini HANYA jalan jika inventory tertutup ---
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX * speed, moveY * speed);
    }
}