using UnityEngine;

namespace StreamHub.Scenes.PersonalWorld
{
  public class WorldManager : MonoBehaviour
  {
    public static WorldManager Instance { get; private set; }
    public MapManager map;
    public PanelManager panel;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      else
        Destroy(this);
    }

    public void Close()
    {
      Instance = null;
    }
  }
}