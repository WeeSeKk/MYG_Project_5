using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
    [SerializeField] UIManager uIManager;
    [SerializeField] public List<SpawnManagerScriptableObject> scriptableObjects;
    [SerializeField] CameraController cameraController;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator LoadScenes(int modelID)
    {
        if (SceneManager.loadedSceneCount <= 1 && modelID != 00)
        {
            uIManager.LoadingScreen();
            yield return SceneManager.LoadSceneAsync("ARScene", LoadSceneMode.Additive);
            cameraController.FollowARCamera();
            uIManager.ChangeSceneUI(1);
            uIManager.LoadingScreen();
            _3DModelsManager _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();
            _3DModelsManager.LoadPreview(modelID);
        }
        else
        {
            uIManager.LoadingScreen();
            yield return SceneManager.UnloadSceneAsync("ARScene");
            uIManager.ChangeSceneUI(2);
            uIManager.LoadingScreen();
        }
    }

    public void ChangeModel(int modelID)
    {
        _3DModelsManager _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();
        _3DModelsManager.LoadPreview(modelID);
    }
}