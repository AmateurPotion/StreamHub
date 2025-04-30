using System.Collections.Generic;
using StreamHub.World;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StreamHub.Scenes.PersonalWorld
{
    public class MapManager : MonoBehaviour
    {
        /// <summary>
        /// 0 floor
        /// 1 structure
        /// </summary>
        public Tilemap[] layers;
        public Tilemap Floors => layers[0];
        public Tilemap Structures => layers[1];
        [SerializeField, Header("Test Build")]
        private SectionInfo testSectionInfo;
        private void Start()
        {
            CreateSection(new Vector2Int(0, 0), testSectionInfo);
        }

        public void CreateSection(Vector2Int sectionPosition, SectionInfo sectionInfo)
        {
            List<Vector3Int> floorPositions = new (), structurePositions = new();
            List<TileBase> floorTiles = new(), structureTiles = new();
            
            for (var y = sectionPosition.y; y < sectionInfo.height + sectionPosition.y; y++)
            {
                // Set Floor Tile
                for (var x = sectionPosition.x; x < sectionInfo.width + sectionPosition.x; x++)
                {
                    floorPositions.Add(new Vector3Int(x, y, 0));
                    floorTiles.Add(sectionInfo.floorTile);
                }
                
                // Set Left/Right Border Wall Tile
                structurePositions.Add(new Vector3Int(sectionPosition.x, y, 0));
                structureTiles.Add(sectionInfo.wallTile);
                structurePositions.Add(new Vector3Int(sectionPosition.x + sectionInfo.width, y, 0));
                structureTiles.Add(sectionInfo.wallTile);
            }
            
            // Set Top/Bottom Border Wall Tile
            for (var x = sectionPosition.x; x < sectionInfo.width + sectionPosition.x; x++)
            {
                structurePositions.Add(new Vector3Int(x, sectionPosition.y, 0));
                structureTiles.Add(sectionInfo.wallTile);
                structurePositions.Add(new Vector3Int(x, sectionPosition.y + sectionInfo.height, 0));
                structureTiles.Add(sectionInfo.wallTile);
            }
            
            // Build Tiles
            Floors.SetTiles(floorPositions.ToArray(), floorTiles.ToArray());
            Structures.SetTiles(structurePositions.ToArray(), structureTiles.ToArray());
        }
    }
}