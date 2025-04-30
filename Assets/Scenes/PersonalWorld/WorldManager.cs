using System;
using UnityEngine;

namespace StreamHub.Scenes.PersonalWorld
{ 
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance { get; private set; } = null;
        public MapManager map;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
                return;
            }
        }

        public void Close()
        {
            Instance = null;
        }
    }
}