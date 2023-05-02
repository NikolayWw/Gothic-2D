using CodeBase.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        Task<GameObject> Instantiate(string path, Vector3 at);

        Task<GameObject> Instantiate(string path);

        Task<T> Load<T>(AssetReference assetReference) where T : class;

        Task<T> Load<T>(string address) where T : class;

        void Cleanup();

        void Initialize();
    }
}