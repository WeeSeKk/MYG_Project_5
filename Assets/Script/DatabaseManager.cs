using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject prefab;


    void Start()
    {
        LoadObject();
    }


    void Update()
    {
        if (Input.GetKeyDown("space"))//test for debug
        {
            LoadObject();
        }
    }

    void LoadObject()
    {
        prefab.LoadAssetAsync().Completed += OnObjectLoaded;
    }

    void OnObjectLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Loaded");
            Instantiate(handle.Result);
        }
        else
        {
            Debug.LogWarning("Can't load the object");
        }
    }
}
