using UnityEngine;

public class MataNgikutin : MonoBehaviour
{
    [Header("Komponen Mata")]
    public Transform bolaMata; // Masukkan objek Bola_Mata_Merah ke sini
    public Transform targetPlayer; // Masukkan Taku ke sini

    [Header("Pengaturan")]
    public float radiusMaksimal = 0.3f; // Seberapa jauh mata bisa melirik (ubah-ubah di Inspector nanti)
    public float kecepatanMelirik = 5f; // Biar lirikan matanya mulus (smooth)

    private Vector3 posisiTengah;

    void Start()
    {
        // Ingat posisi tengah bola mata saat game baru mulai
        if (bolaMata != null)
        {
            posisiTengah = bolaMata.localPosition;
        }

        // Kalau kamu lupa masukin Taku di Inspector, script ini otomatis nyari tag "Player"
        if (targetPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) targetPlayer = playerObj.transform;
        }
    }

    void Update()
    {
        if (bolaMata == null || targetPlayer == null) return;

        // 1. Cari arah dari pusat mata ke player
        Vector3 arahKePlayer = targetPlayer.position - transform.position;

        // Buang sumbu Z karena kita game 2D
        arahKePlayer.z = 0f;

        // 2. Batasi lirikannya biar bola mata gak keluar dari batas putih-putihnya!
        Vector3 targetPosisiLokal = Vector3.ClampMagnitude(arahKePlayer, radiusMaksimal);

        // 3. Pindahkan bola mata dengan mulus ke target posisinya
        bolaMata.localPosition = Vector3.Lerp(bolaMata.localPosition, posisiTengah + targetPosisiLokal, Time.deltaTime * kecepatanMelirik);
    }
}