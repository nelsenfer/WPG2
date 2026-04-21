using UnityEngine;

public class Pintu : MonoBehaviour
{
    [Header("Pengaturan Pintu / Teleport")]
    public bool isLocked;
    public Transform titikTujuan;

    [Header("Syarat Item")]
    public ItemData[] syaratItem;

    [Header("Event Cerita")]
    public GameObject itemRahasia;
    public Sprite potretMC;
    [TextArea] public string dialogTerkunci;
    [TextArea] public string dialogSalahItem;

    [Header("Pengaturan Misi")]
    public bool pertamaKaliDicek = true;
    public bool updateMisiSaatDicek = false;
    // public int misiYangDiharapkan = 0; // Disimpan dulu buat jaga-jaga kalau nanti butuh

    [Header("Gembok Angka (Numpad)")]
    public bool pakaiGembokAngka;
    public string passwordGembok;

    [Header("UI Bantuan")]
    public GameObject promptUI;

    private bool playerDiDekat = false;

    private bool nungguDialogKeluar = false;

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
        // --- FITUR BARU: NUNGGU DIALOG SELESAI BARU LANJUT MISI ---
        if (nungguDialogKeluar && DialogManager.instance != null)
        {
            // Kalau kotak dialognya SUDAH DITUTUP (sedangDialog = false)
            if (DialogManager.instance.sedangDialog == false)
            {
                nungguDialogKeluar = false; // Matikan mode nunggu

                // BARU LANJUTKAN MISINYA SEKARANG!
                if (updateMisiSaatDicek && ObjectiveManager.instance != null)
                {
                    ObjectiveManager.instance.LanjutMisi();
                }
            }
        }

        // Kalau lagi ngobrol atau buka brankas, tombol E nggak berfungsi
        if (DialogManager.instance != null && DialogManager.instance.sedangDialog) return;
        if (NumpadManager.instance != null && NumpadManager.instance.sedangBukaGembok) return;

        if (playerDiDekat && Input.GetKeyDown(KeyCode.E))
        {
            Interaksi();
        }
    }

    void Interaksi()
    {
        if (isLocked)
        {
            // --- 1. UPDATE MISI (Ditaruh paling atas biar gak kena blokir 'return') ---
            if (pertamaKaliDicek)
            {
                pertamaKaliDicek = false; // Matikan saklar ingatan pintu ini
                nungguDialogKeluar = true; // Kasih aba-aba suruh nunggu dialog selesai!
            }

            // --- 2. CEK GEMBOK ANGKA ---
            if (pakaiGembokAngka && NumpadManager.instance != null)
            {
                NumpadManager.instance.BukaNumpad(passwordGembok, this);
                return;
            }

            // --- 3. CEK SYARAT ITEM (Pintu Kunci) ---
            if (syaratItem.Length > 0)
            {
                if (CekPunyaItemDiTas())
                {
                    BukaPintu();
                }
                else
                {
                    // Kalau pertama kali dicek, dialognya bisa ngambil dari dialogSalahItem
                    MunculkanDialog(dialogSalahItem);
                }
                return; // Stop baca kode di bawah
            }

            // --- 4. PINTU MENTOK CERITA (Tanpa Kunci) ---
            MunculkanDialog(dialogTerkunci);
        }
        else
        {
            // PINTU GAK DIKUNCI
            BukaPintu();
        }
    }

    public void BukaPintu()
    {
        isLocked = false;
        if (itemRahasia != null) itemRahasia.SetActive(true);

        if (titikTujuan != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) player.transform.position = titikTujuan.position;
        }
    }

    // --- JEMBATAN UNTUK NUMPAD ---
    public void BukaKunciSukses()
    {
        BukaPintu();
    }

    // --- JEMBATAN UNTUK TAS (INVENTORY) ---
    public bool TerimaItem(ItemData itemDipakai)
    {
        if (!isLocked) return false;

        foreach (ItemData itemButuh in syaratItem)
        {
            if (itemButuh == itemDipakai)
            {
                BukaPintu();
                return true;
            }
        }
        MunculkanDialog(dialogSalahItem);
        return false;
    }

    bool CekPunyaItemDiTas()
    {
        Inventory tas = FindFirstObjectByType<Inventory>();
        if (tas == null) return false;

        foreach (ItemData itemButuh in syaratItem)
        {
            bool ketemu = false;
            foreach (ItemSlot slot in tas.itemList) // <- Udah diperbaiki jadi ItemSlot!
            {
                if (slot.data == itemButuh)
                {
                    ketemu = true;
                    break;
                }
            }
            if (!ketemu) return false;
        }
        return true;
    }

    void MunculkanDialog(string teks)
    {
        if (DialogManager.instance != null)
        {
            BarisDialog[] naskahTunggal = new BarisDialog[1];
            naskahTunggal[0] = new BarisDialog();
            naskahTunggal[0].namaKarakter = "Taku";
            naskahTunggal[0].isiTeks = teks;
            naskahTunggal[0].potretKarakter = potretMC;
            naskahTunggal[0].potretDiKiri = true;
            DialogManager.instance.MulaiDialogNPC(naskahTunggal);
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