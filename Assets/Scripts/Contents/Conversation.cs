using System;
using UnityEngine;

namespace StreamHub.Contents
{
  [CreateAssetMenu(fileName = "new Conversation", menuName = "StreamHub/Conversation", order = 1)]
  public class Conversation : ScriptableObject
  {
    public ConversationFlow[] flows;
  }

  [Serializable]
  public class ConversationFlow
  {
    public string id;
    public string title;
    public string description;
    public string[] options;
  }
}