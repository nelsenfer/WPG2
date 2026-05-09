using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Inventory inventoryPlayer;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // --- INI YANG DI-UPDATE BIAR GAK WARNING LAGI ---
        inventoryPlayer = FindFirstObjectByType<Inventory>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- SATPAM PENGECEKAN KONDISI ---
        bool lagiBukaTas = inventoryPlayer != null && inventoryPlayer.isOpen;
        bool lagiBacaDialog = DialogManager.instance != null && DialogManager.instance.sedangDialog;
        bool lagiBacaTutorial = HintManager.instance != null && HintManager.instance.gameSedangPause;

        // TAMBAHAN SATPAM KERTAS:
        bool lagiBacaKertas = DocumentManager.instance != null && DocumentManager.instance.sedangBaca;

        bool lagiBukaGembok = NumpadManager.instance != null && NumpadManager.instance.sedangBukaGembok;

        // Kalau lagi ngapain aja yang butuh fokus, stop pergerakan!
        if (lagiBukaTas || lagiBacaDialog || lagiBacaTutorial || lagiBacaKertas || lagiBukaGembok)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            return;
        }

        // --- LOGIKA GERAK NORMAL ---
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX * speed, moveY * speed);

        if (moveX != 0 || moveY != 0)
        {
            anim.SetFloat("MoveX", moveX);
            anim.SetFloat("MoveY", moveY);
        }

        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
    }
}