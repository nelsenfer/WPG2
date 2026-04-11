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

    [Header("Pengaturan UI & Dialog")]
    public GameObject panelDialogUI;
    public TMP_Text teksDialogUI;

    // Disembunyikan dari Inspector, otomatis dicari oleh script
    [HideInInspector]
    public GameObject promptUI;

    public string dialogTerkunci = "Terkunci. Butuh sesuatu untuk membukanya.";
    public string dialogSalahItem = "Item ini tidak cocok di sini.";

    private bool playerDiDalam = false;
    private Transform playerTransform;

    void Start()
    {
        // Otomatis mencari Canvas (beserta teks Press E) yang ada di dalam objek Pintu ini
        Canvas canvasChild = GetComponentInChildren<Canvas>(true);
        if (canvasChild != null)
        {
            promptUI = canvasChild.gameObject;
            promptUI.SetActive(false); // Matikan saat awal mulai
        }
    }

    void Update()
    {
        if (playerDiDalam && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                Inventory tasPlayer = FindFirstObjectByType<Inventory>();

                bool punyaSemuaKunci = true;
                foreach (ItemData itemSyarat in syaratItem)
                {
                    if (tasPlayer.CekPunyaItem(itemSyarat) == false)
                    {
                        punyaSemuaKunci = false;
                        break;
                    }
                }

                if (punyaSemuaKunci && syaratItem.Count > 0)
                {
                    foreach (ItemData itemSyarat in syaratItem)
                    {
                        tasPlayer.HapusItemDiamDiam(itemSyarat);
                    }

                    syaratItem.Clear();
                    isLocked = false;

                    TampilkanDialog("Semua kunci cocok. Klik! Pintu terbuka.");
                    if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
                }
                else
                {
                    TampilkanDialog(dialogTerkunci);
                }
            }
            else
            {
                if (titikTujuan != null)
                {
                    if (promptUI != null) promptUI.SetActive(false);
                    playerTransform.position = titikTujuan.position;
                    TampilkanDialog("");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = true;
            playerTransform = collision.transform;

            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null)
            {
                tasPlayer.pintuTerdekat = this;
            }

            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = false;

            Inventory tasPlayer = FindFirstObjectByType<Inventory>();
            if (tasPlayer != null)
            {
                tasPlayer.pintuTerdekat = null;
            }

            TampilkanDialog("");

            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    private void TampilkanDialog(string pesan)
    {
        if (panelDialogUI != null && teksDialogUI != null)
        {
            if (pesan == "")
            {
                panelDialogUI.SetActive(false);
            }
            else
            {
                panelDialogUI.SetActive(true);
                teksDialogUI.text = pesan;
            }
        }
    }
    public bool TerimaItem(ItemData itemYangDiberikan)
    {
        if (!isLocked)
        {
            TampilkanDialog("Pintu ini sudah tidak terkunci.");
            return false; // Item dikembalikan ke tas (tidak jadi dipakai)
        }

        // Cek apakah item yang dipakai pemain cocok dengan syarat pintu
        if (syaratItem.Contains(itemYangDiberikan))
        {
            // Hapus syaratnya karena sudah terpenuhi
            syaratItem.Remove(itemYangDiberikan);

            if (syaratItem.Count == 0)
            {
                isLocked = false;
                TampilkanDialog("Kunci pas! Klik... Pintu berhasil dibuka.");
                if (ObjectiveManager.instance != null) ObjectiveManager.instance.LanjutMisi();
            }
            else
            {
                TampilkanDialog($"Menggunakan {itemYangDiberikan.itemName}. Butuh kunci lainnya...");
            }

            return true; // Item hancur/terpakai dari tas
        }
        else
        {
            // Jika item salah
            TampilkanDialog(dialogSalahItem);
            return false; // Item dikembalikan ke tas
        }
    }
}