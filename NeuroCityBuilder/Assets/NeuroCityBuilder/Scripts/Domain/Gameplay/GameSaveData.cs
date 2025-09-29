using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeuroCityBuilder.Domain.Gameplay
{
    [Serializable]
    public class GameSaveData
    {
        public List<BuildingSaveData> Buildings = new();
        public int Gold;
    }
}

