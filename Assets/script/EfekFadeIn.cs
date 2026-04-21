using System.Collections;
using UnityEngine;

public class EfekFadeIn : MonoBehaviour
{
    [Header("Pengaturan Fade In")]
    public float durasiMuncul = 1.5f; // Butuh 1.5 detik buat muncul sempurna

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (sr != null)
        {
            StartCoroutine(MulaiFadeIn());
        }
    }

    IEnumerator MulaiFadeIn()
    {
        Color warnaBakeneko = sr.color;
        warnaBakeneko.a = 0f;
        sr.color = warnaBakeneko;

        float waktu = 0;
        while (waktu < durasiMuncul)
        {
            waktu += Time.deltaTime;
            warnaBakeneko.a = Mathf.Lerp(0f, 1f, waktu / durasiMuncul);
            sr.color = warnaBakeneko;
            yield return null;
        }

        warnaBakeneko.a = 1f;
        sr.color = warnaBakeneko;
    }
}