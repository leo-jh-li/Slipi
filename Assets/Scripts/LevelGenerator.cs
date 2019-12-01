using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D t_testMapTexture;
    [SerializeField] private float m_tileDist;
    public bool t_autoUpdate;       // Testing variable that indicates whether to re-generate levels when a generation parameter is changed
    private List<GameObject> m_generatedObjects = new List<GameObject>();

    [SerializeField] private Color[] m_colours;
    [SerializeField] private TileType[] m_tileTypes;
    
    [SerializeField] private EntityType[] m_tileEntities;
    [SerializeField] private GameObject[] m_tilePrefabs;
    
    [SerializeField] private GameObject[] m_entityPrefabs;

    public Level GenerateTestMap() {
        foreach(GameObject obj in m_generatedObjects) {
            DestroyImmediate(obj);
        }
        m_generatedObjects.Clear();
        return GenerateLevel(t_testMapTexture);
    }

    public Level GenerateLevel(Texture2D map) {
        Level generatedLevel = new Level();
        generatedLevel.tiles = new Tile[map.height, map.width];
        generatedLevel.tileDist = m_tileDist;

        // Set level tile layout
        for (int y = 0; y < map.height; y += 2) {
            for (int x = 0; x < map.width; x += 2) {
                GenerateTile(ref generatedLevel, map, x, y);
            }
        }

        // Set gates
        for (int y = 0; y < map.height; y += 2) {
            // Check every other tile
            for (int x = 1; x < map.width; x += 2) {
                GenerateTile(ref generatedLevel, map, x, y);
            }
            // Check every tile of the next row
            if (y != map.height - 1) {
                for (int x = 0; x < map.width; x++) {
                    GenerateTile(ref generatedLevel, map, x, y + 1);
                }
            }
        }
        return generatedLevel;
    }

    // Determine the appropriate tile at x, y and set and generate it
    private void GenerateTile(ref Level level, Texture2D map, int x, int y) {
        Tile tile = new Tile();
        Color colour = map.GetPixel(x, map.height - 1 - y);
        tile.tileType = TileType.NONE;
        tile.entityType = EntityType.NONE;
        tile.coord = new Coord(x, y);
        if (colour.a != 0) {
            // Determine tile type
            for (int i = 1; i < m_tileTypes.Length; i++) {
                if (m_colours[i] == colour) {
                    tile.tileType = m_tileTypes[i];
                }
            }
            // If no tile type was read, check for entity
            if (tile.tileType == TileType.NONE) {
                for (int i = 1; i < m_tileEntities.Length; i++) {
                    if (m_colours[m_tileTypes.Length + i] == colour) {
                        tile.entityType = m_tileEntities[i];
                        tile.tileType = TileType.FLOOR;
                    }
                }
            }
        }
        level.tiles[y, x] = tile;
        // Instantiate GameObject(s) for this tile
        if (tile.tileType != TileType.NONE) {
            Vector3 position = transform.position + level.CoordToPos(tile.coord);
            GameObject newTile = Instantiate(m_tilePrefabs[(int)tile.tileType], position, Quaternion.identity, transform);
            level.AddLevelObject(tile.coord, newTile);
            m_generatedObjects.Add(newTile);
            if (tile.tileType == TileType.GATE_OPEN) {
                Gate gate = newTile.GetComponent<Gate>();
                // Set proper gate type for gates; gates on even rows are vertical, gates on odd rows are horizontal
                gate.gateType = y % 2 == 0 ? GateType.VERTICAL : GateType.HORIZONTAL;
                gate.UpdateSprite();
            }
            if (tile.entityType != EntityType.NONE) {
                GameObject newEntity = Instantiate(m_entityPrefabs[(int)tile.entityType], position, Quaternion.identity, transform);
                Piece newPiece = newEntity.GetComponent<Piece>();
                newPiece.coord = tile.coord;
                level.pieces.Add(newPiece);
                level.AddLevelObject(tile.coord, newEntity);
                m_generatedObjects.Add(newEntity);
            }
        }
    }

    public void ClearLevel() {
        foreach(GameObject obj in m_generatedObjects) {
            Destroy(obj);
        }
        m_generatedObjects.Clear();
    }
}
