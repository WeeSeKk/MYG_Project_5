using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Database
{
    public class DatabaseManager : MonoBehaviour
    {
        [SerializeField] List<AssetReferenceGameObject> prefabs;
        List<AssetReferenceGameObject> loadedPrefabs = new List<AssetReferenceGameObject>();
        _3DModelsManager _3DModelsManager;

        public void LoadObject(int type)
        {
            if (!loadedPrefabs.Contains(prefabs[type]))
            {
                prefabs[type].LoadAssetAsync().Completed += OnObjectLoaded;
                loadedPrefabs.Add(prefabs[type]);
            }
            else
            {
                _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();
                _3DModelsManager.InstantiateModel();
            }
        }

        void OnObjectLoaded(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (_3DModelsManager == null)
                {
                    _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();
                }
                _3DModelsManager.InstantiateModel();
            }
            else
            {
                Debug.LogWarning("Can't load the object");
            }
        }
    }
}