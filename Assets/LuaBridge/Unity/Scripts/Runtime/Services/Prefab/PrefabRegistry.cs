using System.Collections.Generic;
using UnityEngine;

namespace Services.Prefab
{
    [CreateAssetMenu(fileName = "PrefabRegistry", menuName = "Dephion/Core/Prefab Registry", order = 1)]
    public class PrefabRegistry : ScriptableObject, IPrefabRegistry
    {
        public List<MonoBehaviour> Prefabs => _prefabs;
        [SerializeField] private List<MonoBehaviour> _prefabs;
    }
}