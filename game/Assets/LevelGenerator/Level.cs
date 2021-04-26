using System.Collections;
using System.Collections.Generic;
using UnityEngine.WSA;

public class Level
{
    private Dictionary<string, LevelTile> grid = new Dictionary<string, LevelTile>();
    public int scaleX = 10;
    public int scaleY = -10;
    public int searchFailureThreshold = 1000;
    public List<LevelTile> candidateTiles = new List<LevelTile>();
    
    //Ask the tile to generator string representations of the grid points it covers e.g. '[1,1]'
    //Then add each of those strings to the grid object as keys and supply the tile as the value
    public void AddTileGridPoints(LevelTile tile)
    {
        var gridPositions = tile.ToStringCoords();
        foreach (var stringCoord in gridPositions)
        {
            grid.Add(stringCoord, tile);
        }
    }

    public bool CoordOccupied(string stringCoord)
    {
        return grid.TryGetValue(stringCoord, out _);
    }
}
