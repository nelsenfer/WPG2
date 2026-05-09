using UnityEngine;

public class MunculSesuaiMisi : MonoBehaviour
{
    [Header("Pengaturan Kemunculan")]
    public int misiMulaiMuncul = 2; // Di misi index ke berapa barang ini muncul?
    public GameObject objekYangDimunculkan; // Tarik Kunci-nya ke sini!

    void Start()
    {
        // Pas game baru mulai, otomatis sembunyikan kuncinya
        if (objekYangDimunculkan != null)
        {
            objekYangDimunculkan.SetActive(false);
        }
    }

    void Update()
    {
        if (ObjectiveManager.instance != null && objekYangDimunculkan != null)
        {
            // Nah, ini yang diganti jadi indeksMisiSaatIni biar cocok sama kodemu!
            if (ObjectiveManager.instance.indeksMisiSaatIni >= misiMulaiMuncul)
            {
                // Munculkan Kunci secara ajaib!
                objekYangDimunculkan.SetActive(true);

                // Hancurkan script ini (bukan kuncinya) biar nggak menuhin memori cek tiap detik
                Destroy(this);
            }
        }
    }
}