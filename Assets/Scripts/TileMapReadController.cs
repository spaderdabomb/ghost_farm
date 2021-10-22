using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class TileMapReadController : MonoBehaviour
{
    //public CinematicCameraController cinematicCameraController;
    [SerializeField] Tilemap tilemapBase;
    [SerializeField] Tilemap tilemapCrops;
    [SerializeField] List<TileData> tileDatas;
    Dictionary<TileBase, TileData> dataFromTiles;

    private void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        
        foreach (TileData tileData in tileDatas)
        {
            foreach(TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public Vector3Int GetGridPosition(Vector2 position)
    {
        // Use for mouse input
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector3 worldPosition = position;
        Vector3Int gridPosition = tilemapBase.WorldToCell(worldPosition);

        return gridPosition;
    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        TileBase tile = tilemapBase.GetTile(gridPosition);

        return tile;
    }

    public TileBase GetTileCrops(Vector3Int gridPosition)
    {
        TileBase tile = tilemapCrops.GetTile(gridPosition);

        return tile;
    }

    public TileData GetTileData(TileBase tilebase)
    {
        TileData tempTile;
        bool keyExists = dataFromTiles.ContainsKey(tilebase);
        if (keyExists)
        {
            tempTile = dataFromTiles[tilebase];
        }
        else
        {
            tempTile = null;
        }

        return tempTile;
    }
}
