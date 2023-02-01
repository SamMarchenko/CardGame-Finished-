
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    [CreateAssetMenu(menuName = "DatabasesSO/Installers/DatabasePrefabsInstaller")]
    public class DatabasePrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameObject _someDatabase;
        [SerializeField] private CardPacksContainer _packsContainer;
        [SerializeField] private ConfigCardsWithAbilities configCardsWithAbilities;

       

        public override void InstallBindings()
        {
            Container.BindInstance(_someDatabase);
            
            Container.BindInstance(_packsContainer);
            _packsContainer.InstallBindings(Container);

            Container.BindInstance(configCardsWithAbilities);

        }
    }
}