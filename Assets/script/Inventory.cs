using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 1. KITA BUAT CLASS BARU UNTUK SLOT INVENTORY
[System.Serializable] // Agar strukturnya bisa terlihat di Inspector Unity
public class ItemSlot
{
    public ItemData data;
    public int jumlah;

    // Ini adalah 'Constructor' untuk membuat slot baru dengan mudah
    public ItemSlot(ItemData itemData, int jumlahAwal)
    {
        data = itemData;
        jumlah = jumlahAwal;
    }
}

public class Inventory : MonoBehaviour
{
    // 2. SEKARANG LIST-NYA MENYIMPAN 'ItemSlot', BUKAN LAGI 'ItemData' MENTAH
    public List<ItemSlot> itemList = new List<ItemSlot>();

    public GameObject inventoryPanel;
    public TMP_Text itemTextUI;
    public bool isOpen = false;
    private int selectedIndex = 0;

    public PuzzleObject puzzleTerdekat = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
            if (isOpen) UpdateInventoryUI();
        }

        if (isOpen && itemList.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.W)) { if (selectedIndex > 0) selectedIndex--; UpdateInventoryUI(); }
            if (Input.GetKeyDown(KeyCode.S)) { if (selectedIndex < itemList.Count - 1) selectedIndex++; UpdateInventoryUI(); }
            if (Input.GetKeyDown(KeyCode.Return)) { UseItem(); }
        }
    }

    // 3. LOGIKA ADD ITEM DIUBAH UNTUK MENGECEK BARANG KEMBAR
    public void AddItem(ItemData newItem)
    {
        bool sudahAda = false;

        // Cek satu-satu isi tas, apakah barang ini sudah pernah diambil?
        foreach (ItemSlot slot in itemList)
        {
            if (slot.data == newItem)
            {
                slot.jumlah++; // Jika sudah ada, tambahkan saja angkanya!
                sudahAda = true;
                break; // Berhenti mencari
            }
        }

        // Jika barang ini benar-benar baru, buat slot baru di bagian bawah list
        if (sudahAda == false)
        {
            itemList.Add(new ItemSlot(newItem, 1));
        }

        if (isOpen) UpdateInventoryUI();
    }

    // 4. LOGIKA UI DIUBAH UNTUK MENAMPILKAN ANGKA "x2", "x3"
    void UpdateInventoryUI()
    {
        if (itemList.Count == 0) { itemTextUI.text = "INVENTORY KOSONG"; return; }

        itemTextUI.text = "INVENTORY:\n\n";
        for (int i = 0; i < itemList.Count; i++)
        {
            // Ambil nama item
            string teksItem = itemList[i].data.itemName;

            // Jika jumlahnya lebih dari 1, tambahkan teks " xJumlah" di belakang namanya
            if (itemList[i].jumlah > 1)
            {
                teksItem += " x" + itemList[i].jumlah;
            }

            // Tampilkan kursor ">"
            if (i == selectedIndex)
                itemTextUI.text += "> " + teksItem + "\n";
            else
                itemTextUI.text += "  " + teksItem + "\n";
        }
    }

    // 5. LOGIKA USE ITEM DIUBAH UNTUK MENGURANGI JUMLAH, BUKAN LANGSUNG MENGHAPUS
    void UseItem()
    {
        ItemData itemToUse = itemList[selectedIndex].data;
        bool itemTerpakai = false; // Penanda apakah item sukses digunakan

        if (puzzleTerdekat != null)
        {
            bool berhasilDipakai = puzzleTerdekat.TerimaItem(itemToUse);
            if (berhasilDipakai == true)
            {
                itemTerpakai = true; // Tandai sukses
            }
        }
        else
        {
            Debug.Log("Item '" + itemToUse.itemName + "' tidak bisa digunakan di sini.");
            // Nanti kamu bisa tambahkan logika untuk item yang bisa dimakan/dipakai sendiri di sini
        }

        // Jika item SUKSES terpakai ke puzzle
        if (itemTerpakai == true)
        {
            itemList[selectedIndex].jumlah--; // Kurangi jumlahnya 1

            // Jika setelah dikurangi ternyata jumlahnya habis (0), baru hapus dari daftar list
            if (itemList[selectedIndex].jumlah <= 0)
            {
                itemList.RemoveAt(selectedIndex);
            }

            // Mencegah error kursor jika menghapus baris paling bawah
            if (selectedIndex >= itemList.Count && itemList.Count > 0)
            {
                selectedIndex = itemList.Count - 1;
            }
            else if (itemList.Count == 0)
            {
                selectedIndex = 0;
            }
        }

        UpdateInventoryUI();
    }
}