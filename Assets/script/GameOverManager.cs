using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [Header("UI Game Over")]
    public GameObject gambarDarah;
    public GameObject panelGameOver;

    // PASTIKAN BARIS INI ADA BIAR GAK ERROR!
    [Header("Sistem Checkpoint")]
    public Transform titikCheckpoint;

    private GameObject takuPlayer;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (gambarDarah != null) gambarDarah.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);

        takuPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    public void MatiDarah()
    {
        StartCoroutine(ProsesMatiBeruntun());
    }

    IEnumerator ProsesMatiBeruntun()
    {
        if (DialogManager.instance != null) DialogManager.instance.sedangDialog = true;
        if (gambarDarah != null) gambarDarah.SetActive(true);
<<<<<<< Updated upstream
=======
        SoundManager.Instance.PlaySound2D("blood");

>>>>>>> Stashed changes
        yield return new WaitForSeconds(2f);
        if (panelGameOver != null) panelGameOver.SetActive(true);
    }

    public void UlangiGame()
    {
        if (gambarDarah != null) gambarDarah.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);

        // Teleport Taku
        if (takuPlayer != null && titikCheckpoint != null)
        {
            takuPlayer.transform.position = titikCheckpoint.position;
        }

        if (DialogManager.instance != null) DialogManager.instance.sedangDialog = false;

        // Reset Kuchisake
        EventDialogKuchisake eventIbuk = FindFirstObjectByType<EventDialogKuchisake>();
        if (eventIbuk != null)
        {
            eventIbuk.ResetEvent();
        }

        // --- FITUR BARU: RESET SEMUA HANTU PATROLI ---
        // (Pakai FindObjectsByType untuk Unity versi baru)
        HantuPatroli[] semuaHantu = FindObjectsByType<HantuPatroli>(FindObjectsSortMode.None);
        foreach (HantuPatroli hantu in semuaHantu)
        {
            hantu.ResetHantu();
        }

        CabinetMover[] semuaKabinet = FindObjectsByType<CabinetMover>(FindObjectsSortMode.None);
        foreach (CabinetMover kabinet in semuaKabinet)
        {
            kabinet.ResetCabinet();
        }
    }

    public void KeluarGame()
    {
        Application.Quit();
    }
}