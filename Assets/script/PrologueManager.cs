using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrologueManager : MonoBehaviour
{
    public TMP_Text teksPrologue;
    [TextArea(3, 5)] 
    public string[] alurCerita; // Bisa diisi banyak paragraf
    public float kecepatanKetik = 0.05f; // Makin kecil makin cepat
    public string namaSceneGame; // Nama scene map utamamu

    private int index = 0;
    private bool sedangNgetik = false;

    void Start()
    {
        StartCoroutine(KetikTeks());
    }

    void Update()
    {
        // Pakai Klik Kiri atau Spasi
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (sedangNgetik)
            {
                // Fitur Skip: Kalau diklik pas lagi ngetik, langsung munculin semua teksnya
                StopAllCoroutines();
                teksPrologue.text = alurCerita[index];
                sedangNgetik = false;
            }
            else
            {
                // Kalau ketikan udah selesai, lanjut ke paragraf berikutnya
                LanjutParagraf();
            }
        }
    }

    IEnumerator KetikTeks()
    {
        sedangNgetik = true;
        teksPrologue.text = ""; // Kosongkan teks di awal

        // Munculin huruf satu per satu
        foreach (char huruf in alurCerita[index].ToCharArray())
        {
            teksPrologue.text += huruf;
            yield return new WaitForSeconds(kecepatanKetik);
        }

        sedangNgetik = false; // Tandai kalau udah selesai ngetik
    }

    void LanjutParagraf()
    {
        index++;

        if (index < alurCerita.Length)
        {
            // Lanjut ke paragraf berikutnya
            StartCoroutine(KetikTeks());
        }
        else
        {
            // Kalau cerita habis, Load Scene Gameplay!
            SceneManager.LoadScene(namaSceneGame);
        }
    }
}