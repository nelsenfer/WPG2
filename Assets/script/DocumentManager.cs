using UnityEngine;
using TMPro;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager instance;

    [Header("UI Kertas")]
    public GameObject panelKertas; // Panel UI Kertas (Background gambar kertas)
    public TMP_Text teksIsiKertas; // Teks di dalam kertas

    // Buat laporan ke Satpam PlayerMovement
    public bool sedangBaca = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        panelKertas.SetActive(false);
    }

    private void Update()
    {
        // Kalau lagi baca kertas, lalu pemain klik kiri / enter / spasi -> TUTUP!
        if (sedangBaca && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            TutupKertas();
        }
    }

    public void BukaKertas(string isi)
    {
        sedangBaca = true;
        panelKertas.SetActive(true);
        teksIsiKertas.text = isi;
    }

    private void TutupKertas()
    {
        sedangBaca = false;
        panelKertas.SetActive(false);
    }
}