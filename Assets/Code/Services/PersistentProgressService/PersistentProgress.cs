using Code.Data;

namespace Code.Services.PersistentProgressService
{
    public class PersistentProgress : IPersistentProgress
    {
        public BlocksProgressData BlocksProgress { get; set; } = new();
    }
}