using StreamHub.Scenes.PersonalWorld;
using UnityEngine;
using UnityEngine.Events;

namespace StreamHub.Prefabs.Interactable
{
  public class InteractableObject : MonoBehaviour
  {
    [SerializeField] protected string title, description;
    public UnityEvent<Player> onInteract;
    public bool descriptable = true;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Material defaultMaterial, highlightMaterial;

    public virtual string Title => title;
    public virtual string Description => description;

    public bool Highlight
    {
      get => spriteRenderer.material == highlightMaterial;
      set => spriteRenderer.material = value ? highlightMaterial : defaultMaterial;
    }

    public virtual void Interact(Player player)
    {
      onInteract.Invoke(player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
        other.GetComponent<Player>().AddFocus(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
        other.GetComponent<Player>().RemoveFocus(this);
    }
  }
}