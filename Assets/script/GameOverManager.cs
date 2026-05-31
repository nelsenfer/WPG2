using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public bool sudahMati = false;

    [Header("UI Game Over")]
    public GameObject gambarDarah;
    public GameObject panelGameOver;

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
        // 1. CEK SATPAM KEMATIAN
        if (sudahMati) return;

        // 2. KUNCI STATUSNYA
        sudahMati = true;

        // 3. CETAK JEJAK DI CONSOLE
        Debug.Log("FUNGSI MATI TERPANGGIL! Coba cek tulisan ini muncul berapa kali.");

        // 4. PANGGIL COROUTINE UI MATI
        StartCoroutine(ProsesMatiBeruntun());
    }

    IEnumerator ProsesMatiBeruntun()
    {
        if (DialogManager.instance != null) DialogManager.instance.sedangDialog = true;
        if (gambarDarah != null) gambarDarah.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (panelGameOver != null) panelGameOver.SetActive(true);
    }

    public void UlangiGame()
    {
        // BUKA KUNCI KEMATIAN AGAR BISA MATI LAGI NANTI
        sudahMati = false;

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

        // Reset Semua Hantu Patroli
        HantuPatroli[] semuaHantu = FindObjectsByType<HantuPatroli>(FindObjectsSortMode.None);
        foreach (HantuPatroli hantu in semuaHantu)
        {
            hantu.ResetHantu();
        }

        // Reset Semua Lemari
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