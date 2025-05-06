using System;

namespace StreamHub.Requests
{
  [Serializable]
  public class DialogRequest : Request
  {
    public string title, description;

    public DialogRequest(string title, string description)
    {
      this.title = title;
      this.description = description;
    }
  }
}