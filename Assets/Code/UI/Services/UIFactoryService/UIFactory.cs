using Code.Player;
using Code.Services;
using Code.Services.StaticData;
using Code.UI.Windows.GameOver;
using Code.UI.Windows.HUD;
using Code.UI.Windows.HUD.PlayerSttus;
using Code.UI.Windows.NetcodeWindow;
using UnityEngine;

namespace Code.UI.Services.UIFactoryService
{
    public class UIFactory : IUIFactory
    {
        private readonly AllServices _allServices;
        private Transform _root;

        public UIFactory(AllServices allServices)
        {
            _allServices = allServices;
        }

        public void CreateUIRoot()
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            GameObject prefab = dataService.WindowData.UIRootPrefab;
            _root = Object.Instantiate(prefab).transform;
        }

        public void CreateStartGameWindow()
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            StartHostClientWindow prefab = dataService.WindowData.StartGameWindowPrefab;
            Object.Instantiate(prefab, _root);
        }

        public void CreateGameOverWindow(string context)
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            GameOverWindow prefab = dataService.WindowData.GameOverWindowPrefab;
            GameOverWindow windowInstance = Object.Instantiate(prefab, _root);
            windowInstance.Construct(context);
        }

        public void CreateDrawWindow()
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            GameObject prefab = dataService.WindowData.DrawWindowPrefab;

            Object.Instantiate(prefab, _root);
        }

        public void CreateHUD()
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            GameObject prefab = dataService.WindowData.HUDPrefab;
            Object.Instantiate(prefab, _root);
        }

        public void CreatePlayerStatusField(Transform root, PlayerHealth playerHealth)
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            PlayerStatusField prefab = dataService.WindowData.PlayerStatusFieldPrefab;
            PlayerStatusField fieldInstance = Object.Instantiate(prefab, root);
            fieldInstance.Construct(playerHealth);
        }

        private TService GetService<TService>() where TService : IService =>
            _allServices.Single<TService>();
    }
}