using UnityEngine;

public class CabinetKill : MonoBehaviour
{
    private CabinetMover mover;

    private void Start()
    {
        mover = GetComponent<CabinetMover>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (mover.isMoving && collision.gameObject.CompareTag("Player"))
        {
            GameOverManager.instance.MatiDarah();
        }
    }
}