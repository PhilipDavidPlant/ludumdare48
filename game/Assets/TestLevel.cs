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
        var firstTile = new LevelTile(3,3,level);
        InstantiateTile(tilePrefabs[0], new LevelGridVector2(0, 0), firstTile, level);
        firstTile.AddEntrance(1,3,EDirection.South);
        
        //### Wire up the straight 1 by 1 tile ###
        var straightTile = CreateTemplate(tilePrefabs[0], 3, 3, level);
        straightTile.AddEntrance(1,0,EDirection.North);
        straightTile.AddEntrance(1,3,EDirection.South);
        level.candidateTiles.Add(straightTile);

        //### Wire up the bend 4 by 3 tile ###
        var bend4X3 = CreateTemplate(tilePrefabs[1], 3, 3, level);
        bend4X3.AddEntrance(1,0,EDirection.North);
        bend4X3.AddEntrance(1,3,EDirection.South);
        bend4X3.AddEntrance(3,1,EDirection.East);
        level.candidateTiles.Add(bend4X3);

        /*//### Wire up the bend 3 by 3 tile ###
        var bend3X3 = CreateTemplate(tilePrefabs[2], 3, 3, level);
        bend3X3.AddEntrance(1,0,EDirection.North);
        bend3X3.AddEntrance(1,3,EDirection.South);
        bend3X3.AddEntrance(0,1,EDirection.West);
        level.candidateTiles.Add(bend3X3);*/
        
        //### Wire up the side straight tile ###
        var sideStraight = CreateTemplate(tilePrefabs[3], 3, 3, level);
        sideStraight.AddEntrance(3,1,EDirection.East);
        sideStraight.AddEntrance(0,1,EDirection.West);
        level.candidateTiles.Add(sideStraight);
        
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
