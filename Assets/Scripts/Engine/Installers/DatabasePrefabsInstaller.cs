
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    [CreateAssetMenu(menuName = "DatabasesSO/Installers/DatabasePrefabsInstaller")]
    public class DatabasePrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameObject _someDatabase;

        public override void InstallBindings()
        {
            Container.BindInstance(_someDatabase);

        }
    }
}