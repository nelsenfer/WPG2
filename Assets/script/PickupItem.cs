using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    public GameObject interactUI;
    private bool canPickup = false;
    private Inventory playerInventory;

    // --- KODE BARU: Penanda apakah item ini memicu pergantian misi ---
    public bool isPemicuMisi = false;

    void Start() { playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            // --- KODE BARU: Cek keamanan (Defensive Programming) ---
            if (itemData != null)
            {
                playerInventory.AddItem(itemData); // Kirim FILE DATA ke inventory

                // --- KODE BARU: Cek apakah item ini menyelesaikan misi ---
                if (isPemicuMisi == true)
                {
                    ObjectiveManager.instance.LanjutMisi();
                }

                interactUI.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                // Muncul peringatan di console, tapi game tidak crash
                Debug.LogWarning("Waduh! Objek item ini belum diisi file ItemData di Inspector!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { canPickup = true; interactUI.SetActive(true); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { canPickup = false; interactUI.SetActive(false); }
    }
}