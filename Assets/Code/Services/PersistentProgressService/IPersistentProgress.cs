using Code.Data;

namespace Code.Services.PersistentProgressService
{
    public interface IPersistentProgress : IService
    {
        BlocksProgressData BlocksProgress { get; set; }
    }
}