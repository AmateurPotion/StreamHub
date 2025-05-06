using TMPro;
using UnityEngine;

namespace StreamHub.Requests
{
  public class RequestReceiver : MonoBehaviour
  {
    [Header("Dialog")] [SerializeField]
    private GameObject dialog;
    [SerializeField] TMP_Text dialogTitle, dialogDescription;
    
    private void Request(Request request)
    {
      if (request is DialogRequest dialogRequest)
      {
        dialogTitle.text = dialogRequest.title;
        dialogDescription.text = dialogRequest.description;
        dialog.SetActive(true);
      }
    }
  }
}