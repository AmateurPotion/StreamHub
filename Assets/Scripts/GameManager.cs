using UnityEngine;

namespace StreamHub
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get; private set; } = null;
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Destroy(this);
        return;
      }
    }
  }
}
