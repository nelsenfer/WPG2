using System.Collections;
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

    [Header("UI Inventory")]
    public GameObject inventoryPanel;
    public TMP_Text itemTextUI;
    public TMP_Text descNameUI;
    public TMP_Text descBodyUI;

    // --- FITUR BARU: UI NOTIFIKASI ITEM ---
    [Header("UI Notifikasi Item Baru")]
    public GameObject panelNotifBaru;
    public TMP_Text teksNotifBaru;

    public bool isOpen = false;
    private int selectedIndex = 0;

    public Pintu pintuTerdekat = null;

    void Start()
    {
        // Pastikan panel notif mati saat awal game
        if (panelNotifBaru != null) panelNotifBaru.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (isOpen && itemList.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedIndex = Mathf.Max(0, selectedIndex - 1);
                UpdateInventoryUI();
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedIndex = Mathf.Min(itemList.Count - 1, selectedIndex + 1);
                UpdateInventoryUI();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                UseItem();
            }
        }
    }

    public void AddItem(ItemData newItem)
    {
        // Panggil notifikasi setiap kali item masuk
        StartCoroutine(TampilkanNotif(newItem.itemName));

        foreach (ItemSlot slot in itemList)
        {
            if (slot.data == newItem) { slot.jumlah++; if (isOpen) UpdateInventoryUI(); return; }
        }
        itemList.Add(new ItemSlot(newItem, 1));
        if (isOpen) UpdateInventoryUI();
    }

    // --- FUNGSI MUNCULKAN NOTIF ---
    private IEnumerator TampilkanNotif(string namaItem)
    {
        if (panelNotifBaru != null && teksNotifBaru != null)
        {
            teksNotifBaru.text = "Mendapatkan: " + namaItem;
            panelNotifBaru.SetActive(true);

            // Notif akan hilang otomatis setelah 2.5 detik
            yield return new WaitForSeconds(2.5f);

            panelNotifBaru.SetActive(false);
        }
    }

    void UpdateInventoryUI()
    {
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

            if (i == selectedIndex)
                itemTextUI.text += "<color=#ffffff>> " + baris + "</color>\n";
            else
                itemTextUI.text += "<color=#888888>  " + baris + "</color>\n";
        }

        ItemData cur = itemList[selectedIndex].data;
        descNameUI.text = cur.itemName;
        descBodyUI.text = cur.itemDescription;
    }

    void UseItem()
    {
        ItemData itemToUse = itemList[selectedIndex].data;

        if (itemToUse.isKertasCatatan)
        {
            ToggleInventory();
            StartCoroutine(BukaKertasDelay(itemToUse.isiKertas));
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

    IEnumerator BukaKertasDelay(string isiSurat)
    {
        yield return null;

        if (DocumentManager.instance != null)
        {
            DocumentManager.instance.BukaKertas(isiSurat);
        }
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
        if (isOpen)
        {
            selectedIndex = 0;
            UpdateInventoryUI();
        }
    }
}