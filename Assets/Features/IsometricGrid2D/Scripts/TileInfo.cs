using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField]
    private int tileType = 0;

    [SerializeField]
    private Vector2Int myIndex;

    [SerializeField]
    private bool filled = false;

    public int TileType
    {
        get { return tileType; }
        set { tileType = value; }
    }

    public Vector2Int MyIndex
    {
        get { return myIndex; }
        set { myIndex = value; }
    }

    public bool Filled
    {
        get { return filled; }
        set { filled = value; }
    }
}
