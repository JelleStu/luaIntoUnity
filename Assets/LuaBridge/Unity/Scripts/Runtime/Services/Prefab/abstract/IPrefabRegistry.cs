using System.Collections.Generic;
using UnityEngine;

namespace Services.Prefab
{
    public interface IPrefabRegistry
    {
        public List<MonoBehaviour> Prefabs { get; }
    }
}