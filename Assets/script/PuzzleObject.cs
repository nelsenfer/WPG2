using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    // List file data yang menjadi syarat puzzle (Bisa di drag & drop di Inspector)
    public List<ItemData> syaratItem = new List<ItemData>();

    public bool TerimaItem(ItemData itemDiberikan)
    {
        if (syaratItem.Count == 0) return false; // Puzzle sudah selesai

        // Cek apakah item yang dipakai player SAMA dengan syarat urutan pertama (index 0)
        if (itemDiberikan == syaratItem[0])
        {
            Debug.Log("Berhasil memproses: " + itemDiberikan.itemName);

            // Hapus syarat pertama dari list karena sudah terpenuhi
            syaratItem.RemoveAt(0);

            // Cek apakah list syarat sudah kosong (semua tahap selesai)
            if (syaratItem.Count == 0)
            {
                Debug.Log("PUZZLE SELESAI! Gerbang terbuka.");
                gameObject.SetActive(false); // Menghilangkan objek pintu
            }

            return true; // Item diterima, Inventory akan menghapusnya dari tas player
        }
        else
        {
            Debug.Log("Item salah atau urutannya belum pas!");
            return false; // Item ditolak
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { collision.GetComponent<Inventory>().puzzleTerdekat = this; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { collision.GetComponent<Inventory>().puzzleTerdekat = null; }
    }
}