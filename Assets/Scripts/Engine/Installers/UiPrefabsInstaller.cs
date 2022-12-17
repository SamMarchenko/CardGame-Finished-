using Engine.UI;
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    [CreateAssetMenu(menuName = "DatabasesSO/Installers/UiPrefabsInstaller")]
    public class UiPrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CanvasView _canvasView;
        [SerializeField] private HudWindowView _hudWindowView;
        public override void InstallBindings()
        {
            Container.Bind<CanvasView>().FromComponentInNewPrefab(_canvasView).AsSingle();
            Container.Bind<HudWindowView>().FromComponentInNewPrefab(_hudWindowView).AsSingle();
        }
    }
}