using System.Collections.Generic;
using StreamHub.Contents;
using StreamHub.Prefabs.Interactable.NPC;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace StreamHub.Scenes.PersonalWorld
{
  public class PanelManager : MonoBehaviour
  {
    [Header("Interaction")] [SerializeField]
    private GameObject interactionPanel;

    [SerializeField] private TMP_Text interactionTitle, interactionDescription;

    public void OpenInteractionPanel(Vector3 position = default, string title = "", string description = "")
    {
      interactionPanel.transform.position = position;
      interactionTitle.text = title;
      interactionDescription.text = description;
      interactionPanel.SetActive(true);
    }

    public void CloseInteractionPanel()
    {
      interactionTitle.text = "";
      interactionDescription.text = "";
      interactionPanel.SetActive(false);
    }

    [Header("Conversation")] [SerializeField]
    private GameObject conversationPanel;

    [SerializeField] private TMP_Text conversationTitle, conversationDescription;
    public List<ConversationFlow> currentFlows;
    public NonePlayableCharacter currentCharacter = null;
    public bool IsConversationOpen => conversationPanel.activeSelf;

    public void OpenConversationPanel(Conversation conversation, NonePlayableCharacter character = null)
    {
      currentFlows = new List<ConversationFlow>(conversation.flows);
      currentCharacter = character;
      conversationPanel.SetActive(true);
      NextFlow();
    }

    public void NextFlow()
    {
      var flow = currentFlows[0];
      currentFlows.RemoveAt(0);
      
      conversationTitle.text = flow.title;
      conversationDescription.text = flow.description;
    }
  }
}