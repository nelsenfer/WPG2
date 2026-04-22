using System.Collections;
using UnityEngine;

public class EventBakenekoDapur : MonoBehaviour
{
    [Header("Kondisi & Barang")]
    public ItemData dataMinyakIkan;
    public int jumlahMinyakDibutuhkan = 3;
    public ItemData dataKunciTerakhir;

    [Header("Aktor")]
    public GameObject bakenekoBaru;

    // --- FITUR BARU: LACI GAMBAR ---
    [Header("Potret Karakter (Wajib Diisi)")]
    public Sprite potretTaku;
    public Sprite potretBakeneko;

    [Header("Naskah")]
    public BarisDialog[] naskahBakenekoDapur;

    private bool eventSelesai = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !eventSelesai)
        {
            Inventory tas = FindFirstObjectByType<Inventory>();
            if (tas != null)
            {
                int jumlahMinyakDiTas = HitungJumlahItem(tas, dataMinyakIkan);

                if (jumlahMinyakDiTas >= jumlahMinyakDibutuhkan)
                {
                    StartCoroutine(MulaiDramaDapur(tas));
                }
            }
        }
    }

    private int HitungJumlahItem(Inventory tas, ItemData itemDicari)
    {
        foreach (ItemSlot slot in tas.itemList)
        {
            if (slot.data == itemDicari) return slot.jumlah;
        }
        return 0;
    }

    IEnumerator MulaiDramaDapur(Inventory tas)
    {
        eventSelesai = true;

        for (int i = 0; i < jumlahMinyakDibutuhkan; i++)
        {
            tas.HapusItemDiamDiam(dataMinyakIkan);
        }

        if (bakenekoBaru != null) bakenekoBaru.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        DialogManager.instance.MulaiDialogNPC(naskahBakenekoDapur);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);

        if (dataKunciTerakhir != null)
        {
            tas.AddItem(dataKunciTerakhir);
            DialogManager.instance.TampilkanDialogBenda("Sistem", "Mendapatkan [Kunci Terakhir]!");
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => !DialogManager.instance.sedangDialog);
        }

        if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
        if (bakenekoBaru != null) bakenekoBaru.SetActive(false);
    }

    // =========================================================================
    // FITUR MAGIC AI: AUTO FILL NASKAH (SEKARANG PLUS GAMBAR!)
    // =========================================================================
    [ContextMenu("✨ ISI OTOMATIS NASKAH BAKENEKO DAPUR ✨")]
    private void AutoIsiNaskah()
    {
        naskahBakenekoDapur = new BarisDialog[5];
        naskahBakenekoDapur[0] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "mana? Dah ketemu kan?", potretKarakter = potretBakeneko };
        naskahBakenekoDapur[1] = new BarisDialog { namaKarakter = "", isiTeks = "(Taku kemudian langsung memberikan barang yang diminta oleh Bakeneko)", potretKarakter = null };
        naskahBakenekoDapur[2] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "nahh... ini yang aku suka.", potretKarakter = potretBakeneko };
        naskahBakenekoDapur[3] = new BarisDialog { namaKarakter = "Taku", isiTeks = "sekarang mana barangnya?", potretKarakter = potretTaku };
        naskahBakenekoDapur[4] = new BarisDialog { namaKarakter = "Bakeneko", isiTeks = "ya sabar lah, nih.....", potretKarakter = potretBakeneko };

        Debug.Log("SULAP AI BERHASIL! Naskah Bakeneko Dapur & GAMBAR sudah terisi otomatis!");
    }
}