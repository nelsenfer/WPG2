using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

   public void PlaySound3D(AudioClip clip, Vector3 pos)
{
    if (clip != null)
    {
        // 1. Buat objek "speaker hantu" baru secara manual
        GameObject speakerSementara = new GameObject("TempAudio_" + clip.name);
        speakerSementara.transform.position = pos;

        // 2. Tambahkan komponen Audio Source
        AudioSource aSource = speakerSementara.AddComponent<AudioSource>();
        aSource.clip = clip;

        // 3. MASUKKAN PENGATURAN 3D DI SINI
        aSource.spatialBlend = 1f; // Full 3D
        aSource.rolloffMode = AudioRolloffMode.Linear; // Agar suara turun perlahan
        aSource.minDistance = 2f; // Jarak suara maksimal
        aSource.maxDistance = 10f; // Jarak suara hilang (Ubah angka ini sesuai ukuran ruanganmu)

        // Opsional: Turunkan sedikit volumenya agar saat banyak trap nyala tidak terlalu bising
        aSource.volume = 0.8f; 

        // 4. Mainkan suara
        aSource.Play();

        // 5. Hancurkan speaker ini tepat setelah durasi suara selesai
        Destroy(speakerSementara, clip.length);
    }
    
    
}
public void PlaySound3D(string soundName, Vector3 pos)
{
    // Mencari AudioClip dari library, lalu mengirimkannya ke fungsi PlaySound3D utama
    PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
}

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}