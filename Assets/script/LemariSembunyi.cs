using UnityEngine;

public class LemariSembunyi : MonoBehaviour
{
    [Header("UI Bantuan")]
    public GameObject promptUI;

    [Header("Status (Dibaca Hantu Nanti)")]
    public bool playerSedangSembunyi = false;

    private bool playerDiDekat = false;
    private GameObject playerObj;

    // Komponen Player
    private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private Collider2D playerCollider;

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;

        if (playerDiDekat && Input.GetKeyDown(KeyCode.E))
        {
            ToggleSembunyi();
        }
    }

    void ToggleSembunyi()
    {
        if (playerObj == null) return;

        playerSedangSembunyi = !playerSedangSembunyi;

        if (playerSprite != null) playerSprite.enabled = !playerSedangSembunyi;
        if (playerMovement != null) playerMovement.enabled = !playerSedangSembunyi;
        if (playerCollider != null) playerCollider.enabled = !playerSedangSembunyi;

        if (playerSedangSembunyi)
        {
            playerObj.transform.position = transform.position;

            if (playerRb != null) playerRb.linearVelocity = Vector2.zero;
            if (playerAnim != null) playerAnim.SetFloat("Speed", 0f);

            if (promptUI != null) promptUI.SetActive(false);
        }
        else
        {
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = true;
            playerObj = collision.gameObject;

            playerSprite = playerObj.GetComponent<SpriteRenderer>();
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            playerRb = playerObj.GetComponent<Rigidbody2D>();
            playerAnim = playerObj.GetComponent<Animator>();
            playerCollider = playerObj.GetComponent<Collider2D>();

            if (promptUI != null && !playerSedangSembunyi) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // --- FIX ERROR: Jangan hapus data Taku kalau dia sedang sembunyi! ---
            if (playerSedangSembunyi) return;

            playerDiDekat = false;
            playerObj = null;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}