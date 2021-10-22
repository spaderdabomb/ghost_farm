using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Plowed
{

}

public class Planted
{
    public string plantType;
    public int plantStage;

    public Planted(string tempPlantType)
    {
        plantType = tempPlantType;
        plantStage = 0;
    }
}

public class CropsManager : MonoBehaviour
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] Tilemap targetTilemap;

    public Dictionary<Vector2Int, Plowed> plowedTile;
    public Dictionary<Vector2Int, Planted> plantedTile;

    public GameSceneController gameSceneController;

    private void Start()
    {
        plowedTile = new Dictionary<Vector2Int, Plowed>();
        plantedTile = new Dictionary<Vector2Int, Planted>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (gameSceneController.fixedUpdateFrame % 300 == 299)
        {
            foreach (KeyValuePair<Vector2Int, Planted> entry in plantedTile)
            {
                if (entry.Value.plantStage < 3)
                {
                    int plantStage = entry.Value.plantStage;
                    entry.Value.plantStage += 1;
                    string tempPlantName = char.ToUpper(entry.Value.plantType[0]) + entry.Value.plantType.Substring(1);
                    string tileName = "plant" + tempPlantName + "_" + plantStage.ToString();
                    Tile tempTile = Resources.Load<Tile>("Tiles/PlantsTiles_1/" + tileName);
                    IncrementPlantedTile(new Vector3Int(entry.Key.x, entry.Key.y, 0), "dummy", plantStage, tempTile);
                }
            }
        }
    }

    public void Plow(Vector3Int position)
    {
        if (plowedTile.ContainsKey((Vector2Int)position))
        {
            return;
        }

        CreatePlowedTile(position);
    }

    public void ClearCropsTile(Vector3Int position)
    {
        targetTilemap.SetTile(position, null);
        plowedTile.Remove((Vector2Int)position);
        plantedTile.Remove((Vector2Int)position);
    }

    public void Plant(Vector3Int position, string cropType)
    {
        if (plantedTile.ContainsKey((Vector2Int)position))
        {
            return;
        }

        CreateSeededTile(position, cropType);
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        Plowed crop = new Plowed();
        plowedTile.Add((Vector2Int)position, crop);

        targetTilemap.SetTile(position, plowed);
    }

    private void CreateSeededTile(Vector3Int position, string cropType)
    {
        Planted crop = new Planted(cropType);
        plantedTile.Add((Vector2Int)position, crop);

        targetTilemap.SetTile(position, seeded);
    }

    private void IncrementPlantedTile(Vector3Int position, string cropType, int cropStage, Tile tileToLoad)
    {
        targetTilemap.SetTile(position, tileToLoad);
    }
}
