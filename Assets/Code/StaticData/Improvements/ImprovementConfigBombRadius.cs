using UnityEngine;

namespace Code.StaticData.Improvements
{
    [CreateAssetMenu(menuName = "Static Data/Improvements/Improvement Bomb Radius", order = 0)]
    public class ImprovementConfigBombRadius : BaseImprovementConfig
    {
        public override ImprovementsId Id { get; protected set; } = ImprovementsId.BombRange;

        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float DelayBeforeExplosion { get; private set; }

    }
}