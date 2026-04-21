using UnityEngine;

public class TriggerCeritaAwal : MonoBehaviour
{
    public BarisDialog[] naskahMonolog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Panggil dialog
            if (DialogManager.instance != null)
            {
                DialogManager.instance.MulaiDialogNPC(naskahMonolog);
            }
            // Hancurkan karpet ini biar dialognya gak ke-trigger 2 kali
            Destroy(gameObject);
        }
    }
}