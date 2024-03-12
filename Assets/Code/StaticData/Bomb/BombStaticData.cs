using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Bomb
{
    [CreateAssetMenu(menuName = "Static Data/Bombs", order = 0)]
    public class BombStaticData : ScriptableObject
    {
        public List<BombConfig> Configs;

        private void OnValidate()
        {
            Configs.ForEach(x => x.OnValidate());
        }
    }
}