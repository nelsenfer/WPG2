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

    // --- FITUR BARU: GEMBOK URUTAN MISI ---
    [Tooltip("Pintu ini HANYA ganti misi jika player sedang di index misi ini. (Misi 1 = 0, Misi 2 = 1, dst). Isi -1 kalau bebas.")]
    public int syaratIndexMisi = -1;

    [Tooltip("Centang ini jika misi ganti saat GAGAL BUKA (Terkunci)")]
    public bool updateMisiSaatDicek = false;

    [Tooltip("Centang ini jika misi ganti saat BERHASIL MASUK/BUKA")]
    public bool updateMisiSaatMasuk = false;

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
        // Fitur Misi 1 (Update Misi setelah dialog terkunci ditutup)
        if (nungguDialogKeluar && DialogManager.instance != null)
        {
            if (DialogManager.instance.sedangDialog == false)
            {
                nungguDialogKeluar = false;

                if (updateMisiSaatDicek && ObjectiveManager.instance != null)
                {
                    // Cek apakah Taku sedang berada di urutan misi yang tepat untuk pintu ini?
                    if (syaratIndexMisi == -1 || ObjectiveManager.instance.indeksMisiSaatIni == syaratIndexMisi)
                    {
                        ObjectiveManager.instance.LanjutMisi();
                    }
                }
            }
        }

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
            if (pertamaKaliDicek)
            {
                pertamaKaliDicek = false;
                nungguDialogKeluar = true;
            }

            if (pakaiGembokAngka && NumpadManager.instance != null)
            {
                NumpadManager.instance.BukaNumpad(passwordGembok, this);
                return;
            }

            if (syaratItem.Length > 0)
            {
                if (CekPunyaItemDiTas())
                {
                    BukaPintu();
                }
                else
                {
                    MunculkanDialog(dialogSalahItem);
                }
                return;
            }

            MunculkanDialog(dialogTerkunci);
        }
        else
        {
            BukaPintu();
        }
    }

    public void BukaPintu()
    {
        isLocked = false;
        if (itemRahasia != null) itemRahasia.SetActive(true);

        if (updateMisiSaatMasuk && ObjectiveManager.instance != null)
        {
            // Cek urutan misi sebelum masuk
            if (syaratIndexMisi == -1 || ObjectiveManager.instance.indeksMisiSaatIni == syaratIndexMisi)
            {
                ObjectiveManager.instance.LanjutMisi();
                updateMisiSaatMasuk = false;
            }
        }

        if (titikTujuan != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // 1. Pindahkan Taku ke ruangan baru
                player.transform.position = titikTujuan.position;

                // --- 2. FITUR BARU: AUTO UPDATE CHECKPOINT! ---
                if (GameOverManager.instance != null)
                {
                    GameOverManager.instance.titikCheckpoint = titikTujuan;
                    Debug.Log("[CHECKPOINT] Spawn point pindah ke pintu baru!");
                }
            }
        }
    }

    public void BukaKunciSukses()
    {
        BukaPintu();
    }

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
            foreach (ItemSlot slot in tas.itemList)
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