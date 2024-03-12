namespace Code.Services.ImprovementsSpawnService
{
    public interface IImprovementsSpawner : IService
    {
        void StartSpawn();

        void StopSpawn();
    }
}