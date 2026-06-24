using UnityEngine;
using System.Collections;

public class CabinetMover : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 2f;

    public bool isMoving = false;
    public bool hasMoved = false; // Penanda bahwa kabinet sudah dipicu

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public void ResetCabinet()
    {
        StopAllCoroutines();

        transform.position = startPosition;
        isMoving = false;
        hasMoved = false; // Buka kembali gembok saat kabinet di-reset
    }

    public void MoveCabinet()
    {
        // Hanya jalankan jika sedang tidak bergerak DAN belum pernah bergerak
        if (!isMoving && !hasMoved)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;
        hasMoved = true; // Langsung kunci agar trigger berikutnya diabaikan

        SoundManager.Instance.PlaySound2D("Kabinet");
        SoundManager.Instance.PlaySound2D("Trigger");

        while (Vector2.Distance(transform.position, targetPoint.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPoint.position;

        isMoving = false;
    }
}