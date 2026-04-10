using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahRuangan : MonoBehaviour
{
    public string namaSceneTujuan;
    public string targetSpawnPoint; // spawn tujuan di scene berikutnya

    public GameObject promptUI;

    private bool playerDiDalam = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDiDalam = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerDiDalam && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pindah ke ruangan: " + namaSceneTujuan);

            if (promptUI != null)
                promptUI.SetActive(false);

            GameManager.Instance.lastSpawnPoint = targetSpawnPoint;
            SceneManager.LoadScene(namaSceneTujuan);
        }
    }
}