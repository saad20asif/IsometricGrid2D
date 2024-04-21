using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int TileType { get; set; } = 0;
    public Vector2Int MyIndex { get; set; }
    public bool Filled { get; set; } = false;
}
