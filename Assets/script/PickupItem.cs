using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Ganti 'string itemName' menjadi 'ItemData itemData'
    public ItemData itemData;

    public GameObject interactUI;
    private bool canPickup = false;
    private Inventory playerInventory;

    void Start() { playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            playerInventory.AddItem(itemData); // Kirim FILE DATA ke inventory
            interactUI.SetActive(false);
            Destroy(gameObject);
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