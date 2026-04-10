using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription; // ← tambahkan ini
    public Sprite icon; // opsional
}