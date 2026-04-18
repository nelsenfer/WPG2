using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Identitas NPC")]
    public string namaNPC;

    [Header("Naskah Obrolan")]
    // Kita pakai tipe BarisDialog yang udah kamu bikin di DialogManager!
    public BarisDialog[] percakapan;

    [Header("UI Bantuan")]
    public GameObject promptUI; // Tulisan "Press E"

    private bool playerDiDekat = false;

    void Start()
    {
        Canvas canvasChild = GetComponentInChildren<Canvas>(true);
        if (canvasChild != null)
        {
            promptUI = canvasChild.gameObject;
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // Kalau lagi dialog/baca kertas/buka gembok, acuhkan tombol E
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;
        if (NumpadManager.instance != null && NumpadManager.instance.sedangBukaGembok) return;

        if (playerDiDekat && Input.GetKeyDown(KeyCode.E))
        {
            // Kalau punya naskah, lempar ke DialogManager!
            if (percakapan.Length > 0 && DialogManager.instance != null)
            {
                DialogManager.instance.MulaiDialogNPC(percakapan);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}