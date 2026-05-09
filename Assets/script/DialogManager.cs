using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct BarisDialog
{
    public string namaKarakter;
    [TextArea] public string isiTeks;
    public Sprite potretKarakter;
    // Variabel ini dibiarkan agar tidak error di Inspector lama,
    // tapi fungsinya diabaikan karena semua dipaksa ke KIRI.
    public bool potretDiKiri;
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("UI Dialog")]
    public GameObject panelDialog;
    public TMP_Text teksNama;
    public TMP_Text teksDialog;
    public Image potretKiri; // SEMUA gambar numpuk di sini

    // Disembunyikan dari Inspector biar rapi.
    // Naskah sekarang dikirim dari NPC / Trigger.
    [HideInInspector]
    public BarisDialog[] alurCerita;

    private int indexKalimat;
    public bool sedangDialog = false;
    private bool modeInteraksiBenda = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        panelDialog.SetActive(false);
        // Fitur Auto-Play dihilangkan agar diatur oleh TriggerCeritaAwal
    }

    private void Update()
    {
        if (sedangDialog && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            if (modeInteraksiBenda)
            {
                SelesaiDialog();
            }
            else
            {
                LanjutKeKalimatBerikutnya();
            }
        }
    }

    public void MulaiDialogNPC(BarisDialog[] percakapanBaru)
    {
        alurCerita = percakapanBaru;
        sedangDialog = true;
        modeInteraksiBenda = false;
        panelDialog.SetActive(true);
        indexKalimat = 0;
        TampilkanKalimat(alurCerita[indexKalimat]);
    }

    public void LanjutKeKalimatBerikutnya()
    {
        indexKalimat++;
        if (indexKalimat < alurCerita.Length) TampilkanKalimat(alurCerita[indexKalimat]);
        else SelesaiDialog();
    }

    private void TampilkanKalimat(BarisDialog baris)
    {
        teksDialog.text = baris.isiTeks;
        if (teksNama != null) teksNama.text = baris.namaKarakter;

        // --- SEMUA GAMBAR DIPAKSA KE KIRI ---
        if (baris.potretKarakter != null)
        {
            if (potretKiri != null)
            {
                potretKiri.gameObject.SetActive(true);
                potretKiri.sprite = baris.potretKarakter;
            }
        }
        else
        {
            if (potretKiri != null) potretKiri.gameObject.SetActive(false);
        }
    }

    public void TampilkanDialogBenda(string nama, string teks, Sprite gambarKarakter = null)
    {
        sedangDialog = true;
        modeInteraksiBenda = true;
        panelDialog.SetActive(true);

        if (teksNama != null) teksNama.text = nama;
        if (teksDialog != null) teksDialog.text = teks;

        // --- GAMBAR BENDA JUGA DIPAKSA KE KIRI ---
        if (gambarKarakter != null)
        {
            if (potretKiri != null)
            {
                potretKiri.gameObject.SetActive(true);
                potretKiri.sprite = gambarKarakter;
            }
        }
        else
        {
            if (potretKiri != null) potretKiri.gameObject.SetActive(false);
        }
    }

    private void SelesaiDialog()
    {
        sedangDialog = false;
        modeInteraksiBenda = false;
        panelDialog.SetActive(false);
    }
}