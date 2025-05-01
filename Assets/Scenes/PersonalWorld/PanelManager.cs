using TMPro;
using UnityEngine;

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
  }
}