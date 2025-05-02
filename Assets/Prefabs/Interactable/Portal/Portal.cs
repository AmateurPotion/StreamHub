using StreamHub.Scenes.World;
using UnityEngine;

namespace StreamHub.Prefabs.Interactable.Portal
{
  public class Portal : InteractableObject
  {
    public Vector2 destination;

    public override string Title => string.Format(title, $"({destination.x}, {destination.y})");
    public override string Description => string.Format(description, $"({destination.x}, {destination.y})");

    public override void Interact(Player player)
    {
      base.Interact(player);

      player.transform.position = destination;
      player.camera.transform.position = new Vector3(destination.x, destination.y, player.camera.transform.position.z);
    }
  }
}