using UnityEngine.UIElements;

public class TileEntrance
{
    public EDirection OpenFaceDirection { get; set; }
    public LevelGridVector2 RelativePosition { get; set; }

    public LevelTile connectedTile = null;

    public TileEntrance(LevelGridVector2 relativePosition, EDirection openFaceDirection)
    {
        OpenFaceDirection = openFaceDirection;
        RelativePosition = relativePosition;
    }

    public TileEntrance Clone()
    {
        return this.MemberwiseClone() as TileEntrance;
    }

    public static bool EntrancesFaceEachOther(TileEntrance t1, TileEntrance t2)
    {
        switch (t1.OpenFaceDirection)
        {
            case EDirection.North when t2.OpenFaceDirection == EDirection.South:
            case EDirection.South when t2.OpenFaceDirection == EDirection.North:
            case EDirection.West when t2.OpenFaceDirection == EDirection.East:
            case EDirection.East when t2.OpenFaceDirection == EDirection.West:
                return true;
            default:
                return false;
        }
    }
}

public enum EDirection
{
    North,
    South,
    East,
    West
}
