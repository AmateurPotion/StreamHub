using UnityEngine;

namespace StreamHub.Scenes.PersonalWorld.Stack
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private StackManager stackManager;

    private void OnJump()
    {
      stackManager.PlaceLine();
    }
  }
}