using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Referensi Panel UI")]
    public GameObject panelOption;   // Tarik panel "Option" ke sini
    public GameObject panelSettings; // Tarik panel "Settings" ke sini
    public GameObject panelCredit;   // Tarik panel "Credit" ke sini

    [Header("Pengaturan Audio (Sama seperti MainMenu)")]
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isPaused = false;

    void Start()
    {
        // Muat volume yang sudah di-save sebelumnya
        LoadVolume();

        // Pastikan semua menu tertutup saat game mulai
        panelOption.SetActive(false);
        panelSettings.SetActive(false);
        panelCredit.SetActive(false);
    }

    void Update()
    {
        // Mengecek jika tombol ESC ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Jika sedang di dalam panel Settings, tekan ESC = Simpan & Tutup Settings
                if (panelSettings.activeSelf)
                {
                    TutupSettingsDanSimpan();
                }
                // Jika sedang di dalam Credit, tekan ESC = Tutup Credit
                else if (panelCredit.activeSelf)
                {
                    panelCredit.SetActive(false);
                }
                // Jika di menu Option biasa, tekan ESC = Lanjut Main
                else
                {
                    LanjutMain();
                }
            }
            else
            {
                // Jika game sedang jalan, tekan ESC = Pause
                PauseGame();
            }
        }
    }

    // --- FUNGSI UNTUK TOMBOL UI PAUSE / HAMBURGER ---
    public void PauseGame()
    {
        isPaused = true;
        panelOption.SetActive(true);
        Time.timeScale = 0f; // Hentikan semua pergerakan dan fisik di Unity
    }

    // --- FUNGSI UNTUK TOMBOL RESUME ---
    public void LanjutMain()
    {
        isPaused = false;
        panelOption.SetActive(false);
        panelSettings.SetActive(false);
        panelCredit.SetActive(false);
        Time.timeScale = 1f; // Kembalikan waktu jadi normal
    }

    // --- FUNGSI UNTUK TOMBOL 'X' DI PANEL SETTINGS ---
    public void TutupSettingsDanSimpan()
    {
        SaveVolume(); // Simpan volume terlebih dahulu
        panelSettings.SetActive(false); // Baru tutup panelnya
    }

    // --- FUNGSI AUDIO BAWAAN DARI TEMANMU ---
    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }

    // --- FUNGSI UNTUK TOMBOL QUIT (KEMBALI KE MAIN MENU) ---
    public void KeluarKeMainMenu()
    {
        Time.timeScale = 1f; // WAJIB dinormalkan sebelum pindah scene!
        SceneManager.LoadScene("NamaSceneMainMenuMu"); // Ganti dengan nama scene menu utama
    }

    public void KeluarDariGame()
    {
        Application.Quit();
    }
}