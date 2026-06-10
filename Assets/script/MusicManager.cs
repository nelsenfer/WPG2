using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

	[SerializeField]
	private MusicLibrary musicLibrary;
	[SerializeField]
    private AudioSource musicSource;

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

public void PlayMusic(string trackName, float fadeDuration = 0.5f)
{
    // 1. Ambil data lagunya dulu dari library
    AudioClip nextTrack = musicLibrary.GetClipFromName(trackName);

    // 2. CEK: Jika lagu yang mau diputar SAMA dengan lagu yang sedang nyala, batalkan perintahnya!
    if (musicSource.clip == nextTrack && musicSource.isPlaying)
    {
        return; // Berhenti di sini, jangan lakukan crossfade atau restart
    }

    // 3. Jika lagunya beda, baru lakukan crossfade seperti biasa
    StartCoroutine(AnimateMusicCrossfade(nextTrack, fadeDuration));
}

	IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
	{
		float percent = 0;
		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / fadeDuration;
			musicSource.volume = Mathf.Lerp(1f, 0, percent);
			yield return null;
		}

		musicSource.clip = nextTrack;
		musicSource.Play();

		percent = 0;
		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / fadeDuration;
			musicSource.volume = Mathf.Lerp(0, 1f, percent);
			yield return null;
		}
	}
}
