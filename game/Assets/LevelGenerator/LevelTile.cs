using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class LevelTile
{
    public LevelGridVector2 position = new LevelGridVector2(0,0);
    private LevelGridVector2 _size;
    public List<TileEntrance> entrances = new List<TileEntrance>();
    private Level _levelReference;
    public Action<LevelGridVector2, LevelTile> CreateTileAction;
    public LevelTile(int sizeX,int sizeY, Level levelReference)
    {
        _size = new LevelGridVector2(sizeX,sizeY);
        _levelReference = levelReference;
    }

    public void AddEntrance(int relativePositionX, int relativePositionY, EDirection openFaceDirection)
    {
        var entrancePosition = new LevelGridVector2(relativePositionX, relativePositionY);
        var entrance = new TileEntrance(entrancePosition, openFaceDirection);
        entrances.Add(entrance);
    }

    public List<string> ToStringCoords()
    {
        List<string> gridPoints = new List<string>();
        for (int x = 0; x < _size.X; x++)
        {
            for (int y = 0; y < _size.Y; y++)
            {
                gridPoints.Add('[' + (position.X + x).ToString() + ',' + (position.Y + y) + ']');
            }
        }

        return gridPoints;
    }

    public void GenerateNeighbours()
    {
        foreach (var entrance in entrances)
        {
            if (entrance.connectedTile == null)
            {
                int failureThreshold = 0;
                while (true)
                {
                    var candidateTile = PickCandidateTileAtRandom();
                    var matchedEntrance = FindMatchedEntrance(entrance, candidateTile);
                    if (matchedEntrance != null)
                    {
                        var candidatePosition = FindPositionToAlignTileToEntrance(entrance, matchedEntrance);
                        candidateTile.position.X = candidatePosition.X;
                        candidateTile.position.Y = candidatePosition.Y;
                        if (TileCanOccupyPosition(candidateTile))
                        {
                            entrance.connectedTile = candidateTile;
                            matchedEntrance.connectedTile = this;
                            _levelReference.AddTileGridPoints(candidateTile);
                            candidateTile.CreateTile();
                            break;
                        }
                    }

                    // There is a chance that we never find a matching piece to fill an entrance
                    // If we have been searching for a while with no matches we will stop searching
                    // and seal the entrance (with a model that looks like rocks)
                    failureThreshold++;
                    if (failureThreshold > _levelReference.searchFailureThreshold)
                    {
                        SealEntrance();
                        break;
                    }
                }
            }
        }
    }
    private TileEntrance FindMatchedEntrance(TileEntrance entrance,LevelTile candidateTile)
    {
        foreach (var candidateEntrance in candidateTile.entrances)
        {
            if (TileEntrance.EntrancesFaceEachOther(entrance,candidateEntrance))
            {
                return candidateEntrance;
            }
        }

        return null;
    }

    private LevelTile PickCandidateTileAtRandom()
    {
        var list = _levelReference.candidateTiles;
        var random = new Random();
        var index = random.Next(list.Count);
        
        var templateClone = list[index].MemberwiseClone() as LevelTile;

        if (templateClone != null)
        {
            templateClone.entrances = new List<TileEntrance>();
            foreach (var entrance in list[index].entrances)
            {
                templateClone.entrances.Add((entrance.Clone()));
            }
            
        }

        return templateClone;
    }

    private void SealEntrance()
    {
        Debug.Log("Need to seal entrance");
    }

    private LevelGridVector2 FindPositionToAlignTileToEntrance(TileEntrance candidateEntrance,TileEntrance matchedEntrance)
    {
        var absoluteEntrancePositionX = position.X + candidateEntrance.RelativePosition.X;
        var absoluteEntrancePositionY = position.Y + candidateEntrance.RelativePosition.Y;

        var positionToAlignNewPieceX = absoluteEntrancePositionX - matchedEntrance.RelativePosition.X;
        var positionToAlignNewPieceY = absoluteEntrancePositionY - matchedEntrance.RelativePosition.Y;
        
        return new LevelGridVector2(
            positionToAlignNewPieceX,
            positionToAlignNewPieceY
        );
    }

    private bool TileCanOccupyPosition(LevelTile candidateTile)
    {
        return candidateTile.ToStringCoords().All(stringCoord => !_levelReference.CoordOccupied(stringCoord));
    }

    private void CreateTile()
    {
        CreateTileAction(position,this);
    }

}


