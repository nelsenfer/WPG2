using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HintManager : MonoBehaviour
{
    public static HintManager instance;

    [Header("UI Panel Biasa")]
    public GameObject hintPanelBiasa;
    public TMP_Text teksHintBiasa;
    public Image iconHintBiasa;

    [Header("UI Panel Besar (Tutorial WASD dsb)")]
    public GameObject hintPanelBesar;
    public TMP_Text teksHintBesar;
    public Image iconHintBesar;

    // Variabel buat ngetrack apakah game lagi di-pause
    private bool gameSedangPause = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SembunyikanSemuaHint();
    }

    private void Update()
    {
        // Kalau game lagi di-pause, tunggu pemain pencet Enter atau Spasi
        if (gameSedangPause)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                LanjutkanGame();
            }
        }
    }

    // Tambahan: Ada parameter baru "bekukanGame"
    public void TampilkanHint(string pesan, Sprite iconTombol = null, bool pakaiPanelBesar = false, bool bekukanGame = false)
    {
        StopAllCoroutines();
        SembunyikanSemuaHint();

        GameObject panelAktif = pakaiPanelBesar ? hintPanelBesar : hintPanelBiasa;
        TMP_Text teksAktif = pakaiPanelBesar ? teksHintBesar : teksHintBiasa;
        Image iconAktif = pakaiPanelBesar ? iconHintBesar : iconHintBiasa;

        if (panelAktif != null) panelAktif.SetActive(true);
        if (teksAktif != null) teksAktif.text = pesan;

        if (iconAktif != null)
        {
            if (iconTombol != null)
            {
                iconAktif.gameObject.SetActive(true);
                iconAktif.sprite = iconTombol;
            }
            else
            {
                iconAktif.gameObject.SetActive(false);
            }
        }

        // Kalau dicentang "bekukanGame", maka waktu dihentikan!
        if (bekukanGame)
        {
            Time.timeScale = 0f; // Menghentikan semua pergerakan/fisika
            gameSedangPause = true;
        }
    }

    public void SembunyikanSemuaHint()
    {
        if (hintPanelBiasa != null) hintPanelBiasa.SetActive(false);
        if (hintPanelBesar != null) hintPanelBesar.SetActive(false);
    }

    // Fungsi untuk memutar waktu kembali saat tombol ditekan
    private void LanjutkanGame()
    {
        Time.timeScale = 1f; // Kembalikan kecepatan waktu jadi normal (1)
        gameSedangPause = false;
        SembunyikanSemuaHint(); // Langsung tutup panelnya
    }
}