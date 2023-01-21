using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    [CreateAssetMenu(menuName = "DatabasesSO/Installers/PrefabsInstaller")]
    public class PrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CardView _cardView;

        public override void InstallBindings()
        {
            Container.BindInstance(_cardView);
        }
    }
}