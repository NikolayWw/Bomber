using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Improvements
{
    [CreateAssetMenu(menuName = "Static Data/Improvements/Static Data", order = -2)]
    public class ImprovementsStaticData : ScriptableObject
    {
        public List<BaseImprovementConfig> Configs;
    }
}