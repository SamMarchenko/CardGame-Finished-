using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    [CreateAssetMenu(menuName = "DatabasesSO/Installers/PrefabsInstaller")]
    public class PrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CardView _cardView;
        [SerializeField] private PlayerHandView _playerHandView;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_cardView);
            Container.BindInstance(_playerHandView);
        }
    }
}