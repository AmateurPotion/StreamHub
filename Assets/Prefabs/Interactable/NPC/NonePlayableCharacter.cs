using StreamHub.Contents;
using StreamHub.Scenes.PersonalWorld;
using UnityEngine.Events;

namespace StreamHub.Prefabs.Interactable.NPC
{
  public class NonePlayableCharacter : InteractableObject
  {
    public UnityEvent<string, string> onAnswer;
    public Conversation conversation;

    public override void Interact(Player player)
    {
      base.Interact(player);
      WorldManager.Instance.panel.OpenConversationPanel(conversation, this);
    }

    public void Select(string id, string option)
    {
      onAnswer.Invoke(id, option);
    }
  }
}