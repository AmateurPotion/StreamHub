using StreamHub.Scenes.PersonalWorld;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace StreamHub.Prefabs.Interactable.NPC
{
  public class DyePool : InteractableObject
  {
    public Color color;
    public SpriteResolver resolver;

    public void SetSpriteLabel(string label)
    {
      resolver.SetCategoryAndLabel(resolver.GetCategory(), label);
    }

    public override void Interact(Player player)
    {
      base.Interact(player);
      player.color = color;
    }
  }
}