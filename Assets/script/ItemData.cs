using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite icon;

    // --- TAMBAHAN BARU UNTUK KERTAS ---
    public bool isKertasCatatan = false; // Centang kalau ini item kertas
    [TextArea(5, 10)] public string isiKertas; // Isi tulisan di dalam kertasnya
}