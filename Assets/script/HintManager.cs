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

    // UBAH JADI PUBLIC: Biar Player tau kalau tutorial lagi menghalangi layar
    public bool gameSedangPause = false;

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
        if (gameSedangPause)
        {
            // --- INI YANG DI-UPDATE ---
            // KeyCode.Space dihapus, diganti jadi Input.GetMouseButtonDown(0) untuk Klik Kiri
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                LanjutkanGame();
            }
        }
    }

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

        // Kalau trigger-nya diset "bekukanGame" di Inspector
        if (bekukanGame)
        {
            gameSedangPause = true; // Kita nyalakan flag-nya (tanpa Time.timeScale = 0)
        }
    }

    public void SembunyikanSemuaHint()
    {
        if (hintPanelBiasa != null) hintPanelBiasa.SetActive(false);
        if (hintPanelBesar != null) hintPanelBesar.SetActive(false);
    }

    private void LanjutkanGame()
    {
        gameSedangPause = false;
        SembunyikanSemuaHint();
    }
}