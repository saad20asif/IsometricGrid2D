using UnityEngine;

public enum Orientation
{
    Horizontal,
    Vertical
}
[CreateAssetMenu(fileName = "Orientation", menuName = "ScriptableObjects/OrientationSO", order = 2)]
public class OrientationSo : ScriptableObject
{
    public Orientation CurrentOrientation;
}
