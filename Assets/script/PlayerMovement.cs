using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Inventory inventoryPlayer; 
    
    // Tambahkan referensi Animator
    private Animator anim; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventoryPlayer = GetComponent<Inventory>(); 
        
        // Mengambil komponen Animator dari Player
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (inventoryPlayer.isOpen == true)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f); // Beritahu animasi untuk diam
            return; 
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        // Menggerakkan player
        rb.linearVelocity = new Vector2(moveX * speed, moveY * speed);

        // --- LOGIKA ANIMASI ---
        
        // Cek apakah player sedang menekan tombol gerak
        if (moveX != 0 || moveY != 0) 
        {
            // Kirim arah X dan Y ke Animator agar karakter menghadap arah yang benar
            anim.SetFloat("MoveX", moveX);
            anim.SetFloat("MoveY", moveY);
        }

        // Kirim kecepatan player ke Animator (jika lebih dari 0.1, dia akan berubah dari Idle ke Walk)
        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
    }
}