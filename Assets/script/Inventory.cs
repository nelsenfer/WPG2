using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;      // Mengecek apakah slot sudah terisi
    public Image[] slots;      // Tempat menaruh 5 gambar slot UI tadi
    public Sprite itemSprite;  // Gambar item saat masuk ke inventory

    // Fungsi ini akan dipanggil saat player mengambil item
    public void AddItem()
    {
        for (int i = 0; i < isFull.Length; i++)
        {
            // Mencari slot pertama yang masih kosong (false)
            if (isFull[i] == false)
            {
                isFull[i] = true; // Tandai slot menjadi penuh
                slots[i].sprite = itemSprite; // Munculkan gambar item di slot tersebut

                // Ubah warna menjadi solid (jika sebelumnya transparan/kosong)
                slots[i].color = new Color(1, 1, 1, 1);
                break; // Hentikan pencarian setelah menemukan 1 slot kosong
            }
        }
    }
}