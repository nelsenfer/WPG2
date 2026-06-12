using UnityEngine;

public class TriggerCeritaAwal : MonoBehaviour
{
    [Header("Potret Karakter")]
    public Sprite potretTaku; // Masukkan potongan gambar Taku (Opening_0) ke sini

    [Header("Naskah Awal Game")]
    public BarisDialog[] naskahMonolog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Panggil dialog
            if (DialogManager.instance != null)
            {
                DialogManager.instance.MulaiDialogNPC(naskahMonolog);
            }

            // Hancurkan kotak gaib ini biar dialognya gak ke-trigger 2 kali
            Destroy(gameObject);
        }
    }

    // =========================================================================
    // FITUR MAGIC AI: AUTO FILL NASKAH PROLOG
    // =========================================================================
    [ContextMenu("✨ ISI OTOMATIS NASKAH PROLOG ✨")]
    private void AutoIsiNaskah()
    {
        naskahMonolog = new BarisDialog[7];

        naskahMonolog[0] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Hmmm... baru melihat luarnya pun firasatku dah beda dari rumah – rumah sebelumnya..", potretKarakter = potretTaku };
        naskahMonolog[1] = new BarisDialog { namaKarakter = "", isiTeks = "(Taku masuk ke mansion itu untuk berniat merekam pengalaman rumah kosong seperti biasa. Namun, suatu ketika saat sedang berjalan tiba – tiba kameranya mati.)", potretKarakter = null };
        naskahMonolog[2] = new BarisDialog { namaKarakter = "Taku", isiTeks = "What the.... ini kenapa lagi kamera gk bisa nyala.", potretKarakter = potretTaku };
        naskahMonolog[3] = new BarisDialog { namaKarakter = "", isiTeks = "(Taku teringat bahwa barang berharganya ia tinggalkan di mobil.)", potretKarakter = null };
        naskahMonolog[4] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Sial HP ku dan batrei cadanganku ada di mobil.", potretKarakter = potretTaku };
        naskahMonolog[5] = new BarisDialog { namaKarakter = "", isiTeks = "(Taku mulai kesal, akan tetapi ia berusaha untuk melanjutkan perjalanan.)", potretKarakter = null };
        naskahMonolog[6] = new BarisDialog { namaKarakter = "Taku", isiTeks = "Baiklah kita akan eksplorasi tempat ini. Tapi pertama kita cek pintu itu.", potretKarakter = potretTaku };

        Debug.Log("SULAP AI BERHASIL! Naskah Prolog sudah terisi otomatis!");
    }
}