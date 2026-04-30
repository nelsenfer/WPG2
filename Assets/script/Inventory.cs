using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ItemSlot
{
    public ItemData data;
    public int jumlah;
    public ItemSlot(ItemData itemData, int jumlahAwal) { data = itemData; jumlah = jumlahAwal; }
}

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> itemList = new List<ItemSlot>();

    public GameObject inventoryPanel;
    public TMP_Text itemTextUI;   // teks daftar item
    public TMP_Text descNameUI;   // nama item yang dipilih
    public TMP_Text descBodyUI;   // keterangan item yang dipilih

    public bool isOpen = false;
    private int selectedIndex = 0;

    public Pintu pintuTerdekat = null;

    void Update()
    {
        // Pakai tombol I untuk buka/tutup tas
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (isOpen && itemList.Count > 0)
        {
            // Pindah ke Atas (Bisa W atau Panah Atas)
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedIndex = Mathf.Max(0, selectedIndex - 1);
                UpdateInventoryUI();
            }

            // Pindah ke Bawah (Bisa S atau Panah Bawah)
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedIndex = Mathf.Min(itemList.Count - 1, selectedIndex + 1);
                UpdateInventoryUI();
            }

            // Eksekusi / Pakai Item (Bisa Enter, Spasi, atau E)
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                UseItem();
            }
        }
    }

    public void AddItem(ItemData newItem)
    {
        foreach (ItemSlot slot in itemList)
        {
            if (slot.data == newItem) { slot.jumlah++; if (isOpen) UpdateInventoryUI(); return; }
        }
        itemList.Add(new ItemSlot(newItem, 1));
        if (isOpen) UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        // -- Daftar item (panel kiri/utama) --
        if (itemList.Count == 0)
        {
            itemTextUI.text = "[ TAS KOSONG ]";
            descNameUI.text = "—";
            descBodyUI.text = "";
            return;
        }

        itemTextUI.text = "";
        for (int i = 0; i < itemList.Count; i++)
        {
            string baris = itemList[i].data.itemName;
            if (itemList[i].jumlah > 1) baris += "  x" + itemList[i].jumlah;

            // Tambahkan Panah (Cursor) biar jelas kelihatan!
            // Ganti simbol ▶ pakai tanda > biar support di semua Font!
            if (i == selectedIndex)
                itemTextUI.text += "<color=#ffffff>> " + baris + "</color>\n";
            else
                itemTextUI.text += "<color=#888888>  " + baris + "</color>\n";
        }

        // -- Keterangan item yang dipilih (panel bawah) --
        ItemData cur = itemList[selectedIndex].data;
        descNameUI.text = cur.itemName;
        descBodyUI.text = cur.itemDescription;
    }

    void UseItem()
    {
        ItemData itemToUse = itemList[selectedIndex].data;

        // --- LOGIKA BACA SURAT ---
        if (itemToUse.isKertasCatatan)
        {
            if (DocumentManager.instance != null)
            {
                DocumentManager.instance.BukaKertas(itemToUse.isiKertas);
            }

            // PENTING: Tutup tas otomatis biar nggak nutupin pop-up surat!
            ToggleInventory();
            return;
        }

        bool itemTerpakai = false;

        if (pintuTerdekat != null)
        {
            if (pintuTerdekat.TerimaItem(itemToUse)) itemTerpakai = true;
        }
        else
        {
            descBodyUI.text = "> " + itemToUse.itemName + " tidak bisa\n  digunakan di sini.";
        }

        if (itemTerpakai)
        {
            itemList[selectedIndex].jumlah--;
            if (itemList[selectedIndex].jumlah <= 0) itemList.RemoveAt(selectedIndex);
            if (selectedIndex >= itemList.Count) selectedIndex = Mathf.Max(0, itemList.Count - 1);
        }

        UpdateInventoryUI();
    }

    public bool CekPunyaItem(ItemData itemDicari)
    {
        foreach (ItemSlot slot in itemList)
        {
            if (slot.data == itemDicari) return true;
        }
        return false;
    }

    public void HapusItemDiamDiam(ItemData itemDihapus)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].data == itemDihapus)
            {
                itemList[i].jumlah--;
                if (itemList[i].jumlah <= 0) itemList.RemoveAt(i);

                // Pastikan kursor nggak bablas kalau item terakhir dihapus
                if (selectedIndex >= itemList.Count) selectedIndex = Mathf.Max(0, itemList.Count - 1);

                if (isOpen) UpdateInventoryUI();
                return;
            }
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        // Reset kursor ke atas pas baru buka tas
        if (isOpen)
        {
            selectedIndex = 0;
            UpdateInventoryUI();
        }
    }
}