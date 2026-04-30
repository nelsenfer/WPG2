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
    }

    public void KeluarGame()
    {
        Application.Quit();
    }
}