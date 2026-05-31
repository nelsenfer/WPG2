using UnityEngine;
using System.Collections;

public class CabinetMover : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 2f;

    public bool isMoving = false;

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
    }

    public void MoveCabinet()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;
        SoundManager.Instance.PlaySound3D("Kabinet", transform.position);

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