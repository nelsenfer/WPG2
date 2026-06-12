using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [TextArea]
    public string pesanHint;
    public Sprite ikonTombol;

    [Header("Pengaturan Tampilan")]
    public bool gunakanPanelBesar = false;
    public bool bekukanGame = false;
    public bool hapusSetelahDilewati = false;

    private bool sedangAktif = false; // Penanda biar gak ter-trigger berkali-kali

    // UBAH JADI STAY: Bakal ngecek terus selama Player berdiri di dalam kotak
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 1. Kalau lagi ada dialog, JANGAN ngapa-ngapain dulu! Tunggu sampai kelar.
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;

        // 2. Kalau dialog udah kelar dan hint belum aktif, baru munculin!
        if (!sedangAktif && collision.CompareTag("Player") && HintManager.instance != null)
        {
            sedangAktif = true;
            HintManager.instance.TampilkanHint(pesanHint, ikonTombol, gunakanPanelBesar, bekukanGame);

            // Kita biarkan objeknya hancur, karena ini tutorial sekali pakai
            if (bekukanGame && hapusSetelahDilewati)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!bekukanGame && collision.CompareTag("Player") && HintManager.instance != null)
        {
            HintManager.instance.SembunyikanSemuaHint();
            sedangAktif = false; // Reset

            if (hapusSetelahDilewati)
            {
                Destroy(gameObject);
            }
        }
    }
}