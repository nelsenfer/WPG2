using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    // --- SISTEM TRIGGER MISI (SUDAH DI-UPGRADE) ---
    [Header("Pengaturan Misi")]
    [Tooltip("Centang HANYA untuk item misi")]
    public bool selesaikanMisiSaatDipungut = false;

    [Tooltip("Jika dicentang, item ini HANYA akan men-trigger misi pada index angka ini (Mulai dari 0)")]
    public int targetIndexMisi = 0;
    // ----------------------------------------------

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
                tasPlayer.AddItem(itemData);
                Debug.Log($"[DEBUG ITEM] {itemData.itemName} masuk tas!");

                // --- PENGAMAN LAPIS DUA ---
                if (selesaikanMisiSaatDipungut && ObjectiveManager.instance != null)
                {
                    // Cek apakah pemain sedang di misi yang tepat untuk item ini
                    if (ObjectiveManager.instance.indeksMisiSaatIni == targetIndexMisi)
                    {
                        ObjectiveManager.instance.LanjutMisi();
                    }
                    else
                    {
                        Debug.LogWarning($"[WARNING] {itemData.itemName} dipungut lebih awal! Misi tidak di-trigger karena sekarang masih Misi ke-{ObjectiveManager.instance.indeksMisiSaatIni}");
                    }
                }
                // --------------------------

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