using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;

namespace NeuroCityBuilder.Infrastructure
{
    public class ObjectDestroyer : IObjectDestroyer
    {
        public void Destroy(GameObject obj)
        {
            if(obj != null)
                Object.Destroy(obj);
        }
    }
}

