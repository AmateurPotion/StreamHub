using UnityEngine;
using UnityEngine.Tilemaps;

namespace StreamHub.World
{
  [CreateAssetMenu(fileName = "new SectionInfo", menuName = "StreamHub/Section Info", order = 0)]
  public class SectionInfo : ScriptableObject
  {
    public TileBase wallTile;
    public TileBase floorTile;
    public TileBase airTile;
    public int width;
    public int height;
  }
}