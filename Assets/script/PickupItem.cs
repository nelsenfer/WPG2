using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    [Header("Pengaturan Misi")]
    [Tooltip("Centang HANYA untuk item misi")]
    public bool selesaikanMisiSaatDipungut = false;

    [Tooltip("Jika dicentang, item ini HANYA akan men-trigger misi pada index angka ini (Mulai dari 0)")]
    public int targetIndexMisi = 0;

    // --- FITUR BARU: BISA MANGGIL BAKENEKO / EVENT LAIN ---
    [Header("Event Spesial (Opsional)")]
    [Tooltip("Masukkan objek Bakeneko ke sini. Dia akan muncul gaib setelah item ini dipungut!")]
    public GameObject objekMunculSetelahDiambil;
    // ------------------------------------------------------

    [HideInInspector]
    public GameObject interactUI;

    private bool canPickup = false;

    void Start()
    {
        Canvas canvasChild = GetComponentInChildren<Canvas>(true);
        if (canvasChild != null)
        {
            interactUI = canvasChild.gameObject;
            interactUI.SetActive(false);
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null)
            {
                // 1. Masukkan ke Tas
                tasPlayer.AddItem(itemData);
                Debug.Log($"[DEBUG ITEM] {itemData.itemName} masuk tas!");

                // 2. Langsung buka Pop-up Kertas!
                if (itemData.isKertasCatatan && DocumentManager.instance != null)
                {
                    DocumentManager.instance.BukaKertas(itemData.isiKertas);
                }

                // 3. Update Misi (kalau ini item misi)
                if (selesaikanMisiSaatDipungut && ObjectiveManager.instance != null)
                {
                    if (ObjectiveManager.instance.indeksMisiSaatIni == targetIndexMisi)
                    {
                        ObjectiveManager.instance.LanjutMisi();
                    }
                    else
                    {
                        Debug.LogWarning($"[WARNING] {itemData.itemName} dipungut lebih awal! Misi tidak di-trigger.");
                    }
                }

                // --- 4. MUNCULKAN BAKENEKO DIAM-DIAM! ---
                if (objekMunculSetelahDiambil != null)
                {
                    objekMunculSetelahDiambil.SetActive(true);
                }

                // 5. Hancurkan kertas di lantai (Tanda Seru juga bakal ikut hancur)
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
            if (interactUI != null) interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
            if (interactUI != null) interactUI.SetActive(false);
        }
    }
}