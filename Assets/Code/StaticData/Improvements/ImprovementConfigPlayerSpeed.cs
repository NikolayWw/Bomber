using UnityEngine;

namespace Code.StaticData.Improvements
{
    [CreateAssetMenu(menuName = "Static Data/Improvements/Improvement Player Speed", order = 0)]
    public class ImprovementConfigPlayerSpeed : BaseImprovementConfig
    {
        public override ImprovementsId Id { get; protected set; } = ImprovementsId.PlayerSpeed;

        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
    }
}