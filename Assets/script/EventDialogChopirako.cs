using System.Collections;
using UnityEngine;

public class EventDialogChopirako : MonoBehaviour
{
    [Header("Naskah & Transaksi Item")]
    public BarisDialog[] naskahChopirako;

    [Tooltip("Masukkan Item Data Boneka Pink di sini untuk dihapus dari tas")]
    public ItemData dataBonekaPink;

    [Tooltip("Masukkan Item Data Kunci Lemari di sini untuk diberikan ke Taku")]
    public ItemData dataKunciLemari;

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

        // 1. Putar Naskah Dialog Chopirako
        DialogManager.instance.MulaiDialogNPC(naskahChopirako);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);

        // 2. Transaksi Item (Taku ngasih Boneka, dapat Kunci)
        Inventory tas = FindFirstObjectByType<Inventory>();
        if (tas != null)
        {
            // Hapus boneka dari tas secara diam-diam
            if (dataBonekaPink != null) tas.HapusItemDiamDiam(dataBonekaPink);

            // Masukkan Kunci Lemari ke tas dan kasih notif pop-up
            if (dataKunciLemari != null)
            {
                tas.AddItem(dataKunciLemari);
                DialogManager.instance.TampilkanDialogBenda("Sistem", "Mendapatkan [Kunci Lemari Chopirako]!");
                yield return new WaitForSeconds(0.1f);
                yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);
            }
        }

        // 3. Update Misi (Ke Misi 7: Buka Lemari / Ambil Minyak)
        if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();

        // 4. Chopirako Menghilang sambil senyum
        gameObject.SetActive(false);
    }

    // =========================================================================
    // FITUR MAGIC AI: AUTO FILL NASKAH CHOPIRAKO!
    // =========================================================================
    [ContextMenu("✨ ISI OTOMATIS NASKAH CHOPIRAKO ✨")]
    private void AutoIsiNaskah()
    {
        naskahChopirako = new BarisDialog[12];
        naskahChopirako[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "eh adik kecil ngapain kamu disitu, turun turun.", potretDiKiri = true };
        naskahChopirako[1] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "ihihihi, om lucu banget.", potretDiKiri = false };
        naskahChopirako[2] = new BarisDialog { namaKarakter = "Taku", isiTeks = "(eh om? Aku masih 25 tahun?)", potretDiKiri = true };
        naskahChopirako[3] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "kenalin aku Chopirako, aku boleh gk main sama om sebentar.", potretDiKiri = false };
        naskahChopirako[4] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Ya apa? Mau main apa?", potretDiKiri = true };
        naskahChopirako[5] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "ihihihi, suka deh sama om, tapi aku Cuma mau boneka yang om bawa. Boleh gk aku ambil? Itu bonekanya bagus banget.", potretDiKiri = false };
        naskahChopirako[6] = new BarisDialog { namaKarakter = "Taku", isiTeks = "nih ambil aja, suka banget ya?", potretDiKiri = true };
        naskahChopirako[7] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "iya kayak mirip boneka yang pernah dikasih orang tuaku dulu.", potretDiKiri = false };
        naskahChopirako[8] = new BarisDialog { namaKarakter = "Taku", isiTeks = "oohh... hmm okelah kalo kamu seneng. Om lanjut dulu ya.", potretDiKiri = true };
        naskahChopirako[9] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "eh tunggu!!\nhehehe kata orang tuaku kalo berbuat baik harus ada timbal baliknya jadi aku mau kasih petunjuk.", potretDiKiri = false };
        naskahChopirako[10] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "Om pasti nyari minyak lampu buat Bakeneko ya? Sebenarnya itu minyak ikan yang ada di ruangan sebelumnya. Aku kasih kunci pintu lemarinya, om tinggal ambil aja.", potretDiKiri = false };
        naskahChopirako[11] = new BarisDialog { namaKarakter = "Chopirako", isiTeks = "semoga membantu, sampai jumpa lagi....", potretDiKiri = false };

        Debug.Log("SULAP AI BERHASIL! Naskah Chopirako sudah terisi otomatis!");
    }
}