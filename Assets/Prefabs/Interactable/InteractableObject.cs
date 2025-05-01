using StreamHub.Scenes.World;
using UnityEngine;
using UnityEngine.Events;

namespace StreamHub.Scenes.PersonalWorld.Interactable
{
  public class InteractableObject : MonoBehaviour
  {
    public UnityEvent onInteract;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material defaultMaterial, highlightMaterial;

    public bool Highlight
    {
      get => spriteRenderer.material == highlightMaterial;
      set => spriteRenderer.material = value ? highlightMaterial : defaultMaterial;
    }

    public void Interact()
    {
      onInteract.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        Highlight = true;
        other.GetComponent<Player>().CurrentTarget = this;
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (other.CompareTag("Player"))
      {
        Highlight = false;
        other.GetComponent<Player>().CurrentTarget = null;
      }
    }
  }
}