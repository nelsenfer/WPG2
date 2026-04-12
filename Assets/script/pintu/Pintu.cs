using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pintu : MonoBehaviour
{
    [Header("Pengaturan Pintu / Teleport")]
    public bool isLocked = true;
    public Transform titikTujuan;

    [Header("Syarat Item")]
    public List<ItemData> syaratItem = new List<ItemData>();

    [Header("Event Cerita")]
    public GameObject itemRahasia;
    private bool pertamaKaliDicek = true;

    [Header("Pengaturan UI & Dialog")]
    public GameObject panelDialogUI;
    public TMP_Text teksDialogUI;

    [HideInInspector]
    public GameObject promptUI;

    public string dialogTerkunci = "Terkunci. Kurasa aku harus mencari kuncinya di sekitar sini.";
    public string dialogSalahItem = "Item ini tidak cocok di sini.";

    private bool playerDiDalam = false;
    private Transform playerTransform;

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
        if (playerDiDalam && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                // EVENT PERTAMA KALI CEK PINTU (Misi 0 -> Misi 1)
                if (pertamaKaliDicek)
                {
                    pertamaKaliDicek = false;
                    if (itemRahasia != null) itemRahasia.SetActive(true);
                    if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
                    TampilkanDialog(dialogTerkunci);
                    return;
                }

                // =================================================================
                // [MODE 1] AUTO GELEDAH TAS SAAT TEKAN 'E' (SAAT INI SEDANG AKTIF)
                // Jika ingin pakai Mode 2, matikan Mode 1 ini dengan cara menambahkan
                // tanda /* di atas tulisan Inventory dan tanda */ di akhir blok ini.
                // =================================================================

                Inventory tasPlayer = FindFirstObjectByType<Inventory>();
                bool punyaSemuaKunci = true;

                if (tasPlayer != null && syaratItem.Count > 0)
                {
                    foreach (ItemData itemSyarat in syaratItem)
                    {
                        if (tasPlayer.CekPunyaItem(itemSyarat) == false)
                        {
                            punyaSemuaKunci = false;
                            break;
                        }
                    }
                }
                else { punyaSemuaKunci = false; }

                if (punyaSemuaKunci)
                {
                    foreach (ItemData itemSyarat in syaratItem) { tasPlayer.HapusItemDiamDiam(itemSyarat); }
                    syaratItem.Clear();
                    isLocked = false;
                    TampilkanDialog("Kunci pas! Klik... Pintu berhasil dibuka.");
                    if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
                }
                else
                {
                    TampilkanDialog(dialogTerkunci);
                }

                // =================================================================
                // [AKHIR DARI MODE 1]
                // =================================================================


                // =================================================================
                // [MODE 2] MANUAL BUKA DARI DALAM TAS (SAAT INI DIMATIKAN/DICOMMENT)
                // Jika ingin pakai Mode 2: Hapus tanda /* dan */ yang mengapit kode di bawah ini,
                // lalu matikan blok Mode 1 di atas.
                // =================================================================
                /*
                TampilkanDialog("Pintu masih terkunci. Aku harus menggunakan kuncinya dari dalam tas.");
                */
                // =================================================================

            }
            else
            {
                // JIKA PINTU SUDAH TERBUKA -> TELEPORT
                if (titikTujuan != null)
                {
                    if (promptUI != null) promptUI.SetActive(false);
                    playerTransform.position = titikTujuan.position;
                    TampilkanDialog("");
                }
            }
        }
    }

    // =================================================================
    // FUNGSI KHUSUS UNTUK MODE 2 (DIPANGGIL OLEH SCRIPT INVENTORY)
    // Biarkan saja ini menyala, tidak akan error meskipun kamu sedang pakai Mode 1.
    // =================================================================
    public bool TerimaItem(ItemData itemYangDiberikan)
    {
        if (!isLocked)
        {
            TampilkanDialog("Pintu ini sudah tidak terkunci.");
            return false;
        }

        if (syaratItem.Contains(itemYangDiberikan))
        {
            syaratItem.Remove(itemYangDiberikan);

            if (syaratItem.Count == 0)
            {
                isLocked = false;
                TampilkanDialog("Kunci pas! Klik... Pintu berhasil dibuka.");
                if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
            }
            else
            {
                TampilkanDialog($"Menggunakan {itemYangDiberikan.itemName}. Butuh sesuatu yang lain lagi...");
            }
            return true; // Item terpakai
        }
        else
        {
            TampilkanDialog(dialogSalahItem);
            return false; // Item ditolak
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = true;
            playerTransform = collision.transform;

            // Penting untuk Mode 2: Memberitahu tas bahwa pemain sedang di dekat pintu ini
            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null) tasPlayer.pintuTerdekat = this;

            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = false;

            // Memutus koneksi dengan tas saat pemain menjauh
            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null) tasPlayer.pintuTerdekat = null;

            TampilkanDialog("");
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    private void TampilkanDialog(string pesan)
    {
        if (panelDialogUI != null && teksDialogUI != null)
        {
            if (pesan == "") panelDialogUI.SetActive(false);
            else
            {
                panelDialogUI.SetActive(true);
                teksDialogUI.text = pesan;
            }
        }
    }
}