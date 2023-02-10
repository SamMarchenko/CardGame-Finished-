
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
        [SerializeField] private ConfigCardsWithAbilities _configCardsWithAbilities;
        [SerializeField] private CardPresetsDataBase _cardPresetsDataBase;

       

        public override void InstallBindings()
        {
            Container.BindInstance(_someDatabase);
            
            Container.BindInstance(_packsContainer);
            

            Container.BindInstance(_configCardsWithAbilities);
            Container.BindInstance(_cardPresetsDataBase);
            _packsContainer.InstallBindings(Container);

        }
    }
}