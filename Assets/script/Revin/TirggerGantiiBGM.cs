using UnityEngine;

public class TriggerGantiBGM : MonoBehaviour
{
    [Header("Pengaturan BGM")]
    [Tooltip("Ketik nama BGM persis seperti yang ada di MusicLibrary")]
    public string namaBGMMapIni;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Pastikan yang menyentuh garis batas ini adalah Player
        if (collision.CompareTag("Player"))
        {
            // Panggil MusicManager untuk mengganti lagunya dengan efek crossfade
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayMusic(namaBGMMapIni);
            }
        }
    }
}
