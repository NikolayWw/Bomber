using Code.Player;
using Code.Services;
using UnityEngine;

namespace Code.UI.Services.UIFactoryService
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        void CreateStartGameWindow();
        void CreateHUD();
        void CreatePlayerStatusField(Transform root, PlayerHealth playerHealth);
        void CreateDrawWindow();
        void CreateGameOverWindow(string context);
    }
}