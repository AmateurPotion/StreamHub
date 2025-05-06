using StreamHub.Scenes.PersonalWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StreamHub.Prefabs.Interactable.Portals
{
  public class ScenePortal : InteractableObject
  {
    [SerializeField]
    public string sceneName;
    public override void Interact(Player player)
    {
      base.Interact(player);

      SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
  }
}