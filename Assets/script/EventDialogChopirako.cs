using System.Collections;
using UnityEngine;

public class EventDialogChopirako : MonoBehaviour
{
    [Header("Naskah & Transaksi Item")]
    public BarisDialog[] naskahChopirako;
    public ItemData dataBonekaPink;
    public ItemData dataKunciLemari;

    // --- FITUR BARU: LACI GAMBAR POTRET ---
    [Header("Potret Karakter (Wajib Diisi Sebelum Klik Auto Fill)")]
    public Sprite potretTaku;      // Masukkan gambar Taku (Opening) ke sini
    public Sprite potretChopirako; // Masukkan gambar Bocil ke sini

    [HideInInspector]
    public GameObject promptUI;

    private bool sudahMulai = false;
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
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;

        if (playerDiDekat && Input.GetKeyDown(KeyCode.E) && !sudahMulai)
        {
            StartCoroutine(MulaiDramaChopirako());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDekat = true;
            if (promptUI != null && !sudahMulai) promptUI.SetActive(true);
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

    IEnumerator MulaiDramaChopirako()
    {
        sudahMulai = true;
        if (promptUI != null) promptUI.SetActive(false);

        DialogManager.instance.MulaiDialogNPC(naskahChopirako);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);

        Inventory tas = FindFirstObjectByType<Inventory>();
        if (tas != null)
        {
            if (dataBonekaPink != null) tas.HapusItemDiamDiam(dataBonekaPink);
            if (dataKunciLemari != null) tas.AddItem(dataKunciLemari);
        }

        if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
        gameObject.SetActive(false);
    }

    // =========================================================================
    // FITUR MAGIC AI: AUTO FILL NASKAH CHOPIRAKO (SEKARANG PLUS GAMBAR!)
    // =========================================================================
    [ContextMenu("✨ ISI OTOMATIS NASKAH CHOPIRAKO ✨")]
    private void AutoIsiNaskah()
    {
        naskahChopirako = new BarisDialog[13];

        // Perhatikan bagian "potretKarakter = potretTaku" dsb!
        naskahChopirako[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "eh adik kecil ngapain kamu disitu, turun turun.", potretKarakter = potretTaku };
        naskahChopirako[1] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "ihihihi, om lucu banget.", potretKarakter = potretChopirako };
        naskahChopirako[2] = new BarisDialog { namaKarakter = "Taku", isiTeks = "(eh om? Aku masih 25 tahun?)", potretKarakter = potretTaku };
        naskahChopirako[3] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "kenalin aku Chopirako, aku boleh gk main sama om sebentar.", potretKarakter = potretChopirako };
        naskahChopirako[4] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Ya apa? Mau main apa?", potretKarakter = potretTaku };
        naskahChopirako[5] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "ihihihi, suka deh sama om, tapi aku Cuma mau boneka yang om bawa. Boleh gk aku ambil? Itu bonekanya bagus banget.", potretKarakter = potretChopirako };
        naskahChopirako[6] = new BarisDialog { namaKarakter = "Taku", isiTeks = "nih ambil aja, suka banget ya?", potretKarakter = potretTaku };
        naskahChopirako[7] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "iya kayak mirip boneka yang pernah dikasih orang tuaku dulu.", potretKarakter = potretChopirako };
        naskahChopirako[8] = new BarisDialog { namaKarakter = "Taku", isiTeks = "oohh... hmm okelah kalo kamu seneng. Om lanjut dulu ya.", potretKarakter = potretTaku };
        naskahChopirako[9] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "eh tunggu!!\nhehehe kata orang tuaku kalo berbuat baik harus ada timbal baliknya jadi aku mau kasih petunjuk.", potretKarakter = potretChopirako };
        naskahChopirako[10] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "Om pasti nyari minyak lampu buat Bakeneko ya? Sebenarnya itu minyak ikan yang ada di ruangan sebelumnya. Aku kasih kunci pintu lemarinya, om tinggal ambil aja.", potretKarakter = potretChopirako };
        naskahChopirako[11] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "semoga membantu, sampai jumpa lagi....", potretKarakter = potretChopirako };
        naskahChopirako[12] = new BarisDialog { namaKarakter = "", isiTeks = "(Kamu mendapatkan Kunci Lemari Chopirako!)", potretKarakter = null };

        Debug.Log("SULAP AI BERHASIL! Naskah Chopirako dan GAMBARNYA sudah terisi otomatis!");
    }
}