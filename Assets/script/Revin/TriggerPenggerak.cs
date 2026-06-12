using UnityEngine;

public class TriggerPenggerak : MonoBehaviour
{
    public CabinetMover cabinet;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cabinet.MoveCabinet();
        }
    }
}