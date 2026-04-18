using System.Collections.Generic;
using UnityEngine;

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
    [HideInInspector]
    public GameObject promptUI;

    public Sprite potretMC;

    public string dialogTerkunci = "Terkunci. Kurasa aku harus mencari kuncinya di sekitar sini.";
    public string dialogSalahItem = "Item ini tidak cocok di sini.";

    private bool playerDiDalam = false;
    private Transform playerTransform;

    [Header("Gembok Angka (Numpad)")]
    public bool pakaiGembokAngka = false; // Centang ini kalau pintu pakai password
    public string passwordGembok = "1234";

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

        if (playerDiDalam && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                // 1. CEK EVENT PERTAMA KALI
                if (pertamaKaliDicek)
                {
                    pertamaKaliDicek = false;
                    if (itemRahasia != null) itemRahasia.SetActive(true);
                    if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
                    TampilkanDialog(dialogTerkunci);
                    return; // Return kosongan di sini aman, karena fungsi Update() itu void
                }

                // 2. CEK GEMBOK ANGKA DULU (INI YANG TADI KAMU LUPA MASUKIN)
                if (pakaiGembokAngka)
                {
                    if (NumpadManager.instance != null)
                    {
                        NumpadManager.instance.BukaNumpad(passwordGembok, this);
                    }
                    return; // Hentikan script di sini, jangan lanjut ngecek isi tas
                }

                // 3. CEK KUNCI DARI TAS
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
            }
            else
            {
                // JIKA PINTU SUDAH TERBUKA -> TELEPORT
                if (titikTujuan != null)
                {
                    if (promptUI != null) promptUI.SetActive(false);
                    playerTransform.position = titikTujuan.position;
                }
            }
        }
    }

    public bool TerimaItem(ItemData itemYangDiberikan)
    {
        if (!isLocked)
        {
            TampilkanDialog("Pintu ini sudah tidak terkunci.");
            return false;
        }

        // --- INI PERBAIKAN ERROR-NYA ---
        if (pakaiGembokAngka)
        {
            TampilkanDialog("Pintu ini dikunci menggunakan gembok angka, bukan kunci biasa.");
            return false; // WAJIB ada kata false!
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

            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null) tasPlayer.pintuTerdekat = null;

            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    public void BukaKunciSukses()
    {
        isLocked = false;
        TampilkanDialog("Klik! Gembok terbuka.");
        if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
    }

    private void TampilkanDialog(string pesan)
    {
        if (DialogManager.instance != null)
        {
            DialogManager.instance.TampilkanDialogBenda("Taku", pesan, potretMC);
        }
    }
}