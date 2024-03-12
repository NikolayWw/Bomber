using Code.StaticData.Bomb;
using Code.StaticData.CountdownTimer;
using Code.StaticData.Improvements;
using Code.StaticData.Improvements.Spawner;
using Code.StaticData.Player;
using Code.StaticData.Windows;

namespace Code.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        PlayerStaticData PlayerData { get; }
        WindowStaticData WindowData { get; }
        ImprovementsSpawnStaticData ImprovementsSpawnData { get; }
        ImprovementsStaticData ImprovementsData { get; }
        CountdownTimerStaticData CountdownTimerData { get; }
        BombConfig ForBomb(BombId id);
        BaseImprovementConfig ForImprovement(ImprovementsId id);
    }
}