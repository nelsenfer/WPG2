using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [TextArea]
    public string pesanHint;
    public Sprite ikonTombol;

    [Header("Pengaturan Tampilan")]
    public bool gunakanPanelBesar = false;
    public bool bekukanGame = false; // <-- CENTANG INI BIAR GAMENYA BERHENTI!
    public bool hapusSetelahDilewati = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && HintManager.instance != null)
        {
            // Kirim status bekukanGame ke manager
            HintManager.instance.TampilkanHint(pesanHint, ikonTombol, gunakanPanelBesar, bekukanGame);

            // Kalau gamenya beku, trigger ini langsung kita hancurkan (jika dicentang) 
            // biar pas game jalan lagi gak kepanggil dua kali
            if (bekukanGame && hapusSetelahDilewati)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hanya matikan panel kalau gamenya TIDAK beku. 
        // (Kalau beku, panel dimatikan oleh tombol Enter di HintManager).
        if (!bekukanGame && collision.CompareTag("Player") && HintManager.instance != null)
        {
            HintManager.instance.SembunyikanSemuaHint();

            if (hapusSetelahDilewati)
            {
                Destroy(gameObject);
            }
        }
    }
}