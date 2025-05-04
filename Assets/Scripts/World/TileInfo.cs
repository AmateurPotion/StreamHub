using StreamHub.Scenes.PersonalWorld;
using UnityEngine;

namespace StreamHub.World
{
  public class TileInfo : MonoBehaviour
  {
    public float slowdown;
    public int toxicity;

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
        other.gameObject.GetComponent<Player>().AddVisitTile(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
        other.gameObject.GetComponent<Player>().RemoveVisitTile(this);
    }
  }
}