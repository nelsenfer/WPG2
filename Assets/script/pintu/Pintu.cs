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

    [Header("Pengaturan Dialog Sementara")]
    public TMP_Text teksDialogUI;
    public string dialogTerkunci = "Terkunci. Butuh sesuatu untuk membukanya.";
    public string dialogSalahItem = "Item ini tidak cocok di sini.";

    private bool playerDiDalam = false;
    private Transform playerTransform;

    void Update()
    {
        if (playerDiDalam && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                Inventory tasPlayer = FindFirstObjectByType<Inventory>();

                // Mengecek apakah player punya SEMUA item yang dibutuhkan
                bool punyaSemuaKunci = true;
                foreach (ItemData itemSyarat in syaratItem)
                {
                    if (tasPlayer.CekPunyaItem(itemSyarat) == false)
                    {
                        punyaSemuaKunci = false;
                        break; // Langsung berhenti ngecek kalau ada 1 aja yang kurang
                    }
                }

                // Eksekusi jika pemain punya semua kuncinya
                if (punyaSemuaKunci && syaratItem.Count > 0)
                {
                    // Hapus semua kunci yang terpakai dari tas
                    foreach (ItemData itemSyarat in syaratItem)
                    {
                        tasPlayer.HapusItemDiamDiam(itemSyarat);
                    }

                    syaratItem.Clear(); // Kosongkan daftar syarat karena sudah terpenuhi
                    isLocked = false;

                    TampilkanDialog("Semua kunci cocok. Klik! Pintu terbuka.");
                    ObjectiveManager.instance.LanjutMisi();
                }
                else
                {
                    // Jika kurang 1 kunci atau tidak punya sama sekali
                    TampilkanDialog(dialogTerkunci);
                }
            }
            else
            {
                // Pintu sudah tidak terkunci, lakukan teleportasi
                if (titikTujuan != null)
                {
                    playerTransform.position = titikTujuan.position;
                    TampilkanDialog("");
                }
            }
        }
    }

    public bool TerimaItem(ItemData itemDiberikan)
    {
        if (!isLocked) return false;

        if (syaratItem.Count > 0 && itemDiberikan == syaratItem[0])
        {
            syaratItem.RemoveAt(0);

            if (syaratItem.Count == 0)
            {
                isLocked = false;
                TampilkanDialog("Klik! Suara kunci terbuka.");
                ObjectiveManager.instance.LanjutMisi();
            }
            return true;
        }
        else
        {
            TampilkanDialog(dialogSalahItem);
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = true;
            playerTransform = collision.transform;

            // Memberitahu Inventory bahwa player sedang di dekat pintu
            collision.GetComponent<Inventory>().pintuTerdekat = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = false;

            // Memberitahu Inventory bahwa player sudah menjauh dari pintu
            collision.GetComponent<Inventory>().pintuTerdekat = null;
            TampilkanDialog("");
        }
    }

    private void TampilkanDialog(string pesan)
    {
        if (teksDialogUI != null) teksDialogUI.text = pesan;
    }
}