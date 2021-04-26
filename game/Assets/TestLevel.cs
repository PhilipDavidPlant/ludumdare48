using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class TestLevel : MonoBehaviour
{
 
    [SerializeField]
    private Transform[] tilePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        Level level = new Level();

        //Wire first tile
        var firstTile = new LevelTile(1,1,level);
        InstantiateTile(tilePrefabs[0], new LevelGridVector2(0, 0), firstTile, level);
        firstTile.AddEntrance(0,1,EDirection.South);
        
        //### Wire up the straight 1 by 1 tile ###
        var straightTile = CreateTemplate(tilePrefabs[0], 1, 1, level);
        straightTile.AddEntrance(0,0,EDirection.North);
        straightTile.AddEntrance(0,1,EDirection.South);
        level.candidateTiles.Add(straightTile);

        //### Wire up the side 1 by 1 tile ###
        var sideTile1By1 = CreateTemplate(tilePrefabs[2], 1, 1, level);
        sideTile1By1.AddEntrance(0,0,EDirection.West);
        sideTile1By1.AddEntrance(1,0,EDirection.East);
        level.candidateTiles.Add(sideTile1By1);

        //### Wire up the two by two arena ###
        var twoByTwoArena = CreateTemplate(tilePrefabs[1], 2, 2, level);
        twoByTwoArena.AddEntrance(0,0,EDirection.North);
        twoByTwoArena.AddEntrance(1,2,EDirection.South);
        level.candidateTiles.Add(twoByTwoArena);
        
        //### Wire up the two by two arena left side entrance ###
        var twoByTwoArenaLeftSideEntrance = CreateTemplate(tilePrefabs[3], 2, 2, level);
        twoByTwoArenaLeftSideEntrance.AddEntrance(1,0,EDirection.North);
        twoByTwoArenaLeftSideEntrance.AddEntrance(2,0,EDirection.East);
        level.candidateTiles.Add(twoByTwoArenaLeftSideEntrance);
        
        //### Wire up the two by two arena right side entrance ###
        var twoByTwoArenaRightSideEntrance = CreateTemplate(tilePrefabs[4], 2, 2, level);
        twoByTwoArenaRightSideEntrance.AddEntrance(1,0,EDirection.North);
        twoByTwoArenaRightSideEntrance.AddEntrance(0,0,EDirection.West);
        level.candidateTiles.Add(twoByTwoArenaRightSideEntrance);
        
        //### Wire up the two by two arena right side entrance ###
        var twoByTwoArenaRightSideEntranceBottom = CreateTemplate(tilePrefabs[5], 2, 2, level);
        twoByTwoArenaRightSideEntranceBottom.AddEntrance(0,0,EDirection.West);
        twoByTwoArenaRightSideEntranceBottom.AddEntrance(1,2,EDirection.South);
        level.candidateTiles.Add(twoByTwoArenaRightSideEntranceBottom);
        
        //Lastly generate some neighbours for the first tile so that you can't see (as much) pop in
        firstTile.GenerateNeighbours();
        
    }

    private LevelTile CreateTemplate(Transform prefabTile, int sizeX, int sizeY, Level level)
    {
        return new LevelTile(sizeX, sizeY, level)
        {
            CreateTileAction = delegate(LevelGridVector2 position, LevelTile levelTileReference)
            {
                InstantiateTile(prefabTile, position, levelTileReference, level);
            }
        };
    }

    private void InstantiateTile(Transform tilePrefab, LevelGridVector2 position, LevelTile levelTileReference,Level levelReference)
    {
        var tile = Instantiate(
            tilePrefab,
            new Vector3(position.X * levelReference.scaleX, position.Y * levelReference.scaleY, 0),
            Quaternion.identity
        );
        tile.Find("collider").GetComponent<TileDebug>().tile = levelTileReference;
    }

}
