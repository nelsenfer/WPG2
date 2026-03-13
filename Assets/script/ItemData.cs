using UnityEngine;

// Baris ini membuat menu baru saat kamu klik kanan di jendela Project Unity
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    // Cukup simpan nama itemnya saja untuk sekarang
    public string itemName;
}