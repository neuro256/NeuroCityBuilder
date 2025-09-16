using UnityEngine;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameEntryPoint : IStartable
    {
        public void Start()
        {
            Debug.Log("Game started");
        }
    }
}


