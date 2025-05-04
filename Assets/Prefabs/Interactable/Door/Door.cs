using StreamHub.Scenes.PersonalWorld;
using UnityEngine;

namespace StreamHub.Prefabs.Interactable.Door
{
  public class Door : InteractableObject
  {
    [Header("Door")] public bool open;
    [SerializeField] private Sprite openSprite, closedSprite;
    [SerializeField] private Collider2D doorCollider;

    public override void Interact(Player player)
    {
      base.Interact(player);
      open = !open;
      spriteRenderer.sprite = open ? openSprite : closedSprite;
      doorCollider.enabled = !open;
    }
  }
}