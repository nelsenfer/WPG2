using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    [HideInInspector]
    public GameObject interactUI;

    private bool canPickup = false;

    void Start()
    {
        // Otomatis mencari Canvas di dalam Item
        Canvas canvasChild = GetComponentInChildren<Canvas>(true);

        if (canvasChild != null)
        {
            interactUI = canvasChild.gameObject;
            interactUI.SetActive(false);
            Debug.Log($"[DEBUG ITEM] Sukses! UI ditemukan pada item: {gameObject.name}");
        }
        else
        {
            Debug.LogError($"[DEBUG ITEM] ERROR! Canvas tidak ditemukan di dalam item: {gameObject.name}. Cek Hierarchy!");
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null)
            {
                // INI SUDAH BENAR: Pakai AddItem
                tasPlayer.AddItem(itemData);
                Debug.Log($"[DEBUG ITEM] {itemData.itemName} berhasil masuk tas!");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
            Debug.Log($"[DEBUG ITEM] Player MENDEKAT ke {gameObject.name}. Mencoba menyalakan UI...");

            if (interactUI != null)
            {
                interactUI.SetActive(true);
                Debug.Log("[DEBUG ITEM] UI disuruh nyala!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
            Debug.Log($"[DEBUG ITEM] Player MENJAUH dari {gameObject.name}. Mematikan UI...");

            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }
        }
    }
}