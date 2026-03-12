using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private bool canPickup = false;
    public GameObject interactUI; // Slot untuk teks "Press E"
    private Inventory playerInventory;

    void Start()
    {
        // Mencari script inventory pada Player secara otomatis saat game mulai
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        // Jika player di dekat item DAN menekan tombol E
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            playerInventory.AddItem(); // Masukkan ke inventory
            interactUI.SetActive(false); // Sembunyikan teks "Press E"
            Destroy(gameObject); // Hancurkan object item dari dunia
        }
    }

    // Terdeteksi saat player mendekat (masuk ke dalam area collider item)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
            interactUI.SetActive(true); // Munculkan teks "Press E"
        }
    }

    // Terdeteksi saat player menjauh (keluar dari area collider item)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
            interactUI.SetActive(false); // Sembunyikan teks "Press E"
        }
    }
}