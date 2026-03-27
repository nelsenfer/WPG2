using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    // Posisi Z -10 tetap dipertahankan agar kamera tidak masuk ke dalam sprite
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // VARIABEL DIHAPUS: 
    // public float smoothTime = 0.25f; -> Dihapus karena tidak butuh delay
    // private Vector3 velocity = Vector3.zero; -> Dihapus karena tidak butuh perhitungan kecepatan

    void LateUpdate()
    {
        // Langsung mengatur posisi kamera sama persis dengan posisi player ditambah offset.
        // Karena tidak pakai SmoothDamp, kamera akan menempel kaku ke player.
        transform.position = player.position + offset;
    }
}