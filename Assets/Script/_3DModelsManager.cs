using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Database;

public class _3DModelsManager : MonoBehaviour
{
    [SerializeField] public List<SpawnManagerScriptableObject> scriptableObjects;
    [SerializeField] GameObject container;
    [SerializeField] Image image;
    [SerializeField] Canvas canvas;
    [SerializeField] Camera _camera;
    [SerializeField] GameObject modelUI;
    [SerializeField] GameObject modelPreview;
    [SerializeField] Material materialPreview;
    DatabaseManager databaseManager;
    GameObject modelContainer;
    GameObject previewModel;
    Image modelInfo;
    TMP_Text tMP_TextName;
    TMP_Text tMP_TextDescription;
    int currentModelID;
    Vector3 currentPos;
    Quaternion currentRot;

    // Start is called before the first frame update
    void Start()
    {
        databaseManager = GameObject.Find("DatabaseManager").GetComponent<DatabaseManager>();
    }

    public void LoadAssets(Vector3 pos, Quaternion rot)
    {
        currentPos = pos;
        currentRot = rot;
        databaseManager.LoadObject(currentModelID);
    }

    public void InstantiateModel()
    {
        GameObject modelUIContainer;
        GameObject _3DModel;

        modelContainer = Instantiate(container, currentPos, currentRot, this.transform);
        _3DModel = Instantiate(AppManager.instance.scriptableObjects[currentModelID].model3D, currentPos, currentRot, modelContainer.transform);
        modelUIContainer = Instantiate(modelUI, currentPos, currentRot, canvas.transform);

        modelInfo = Instantiate(image, new Vector3(_3DModel.transform.position.x, _3DModel.transform.position.y + 0.4f, _3DModel.transform.position.z), _3DModel.transform.rotation, modelUIContainer.transform);
        modelInfo.gameObject.SetActive(false);

        tMP_TextName = modelInfo.transform.GetChild(1).GetComponent<TMP_Text>();
        tMP_TextDescription = modelInfo.transform.GetChild(2).GetComponent<TMP_Text>();
        tMP_TextName.text = AppManager.instance.scriptableObjects[currentModelID].productName;
        tMP_TextDescription.text = AppManager.instance.scriptableObjects[currentModelID].productDescription;
        Image infoImage = modelInfo.transform.GetChild(0).GetComponent<Image>();
    }

    public GameObject Model()
    {
        return modelContainer;
    }

    public Image modelInfoImage()
    {
        return modelInfo;
    }

    public void HitPosResult()
    {
        Ray r = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit) && hit.transform.tag == "Furniture")
        {
            Debug.Log(hit.transform.gameObject.name);
            EventManager.ShowHideInfo(hit.transform.gameObject);
        }
        else
        {
            EventManager.ScreenTapEvent();
        }
    }

    public void OnTargetLostFound(int type)
    {
        if (type == 0)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void LoadPreview(int modelID)
    {
        Transform previewPlace = GameObject.Find("DefaultPlaneIndicator_URP(Clone)").GetComponent<Transform>();
        GameObject baseIndicator = GameObject.Find("DefaultIndicator");

        if (baseIndicator != null)
        {
            baseIndicator.SetActive(false);
        }

        previewModel = Instantiate(AppManager.instance.scriptableObjects[modelID].model3D, previewPlace.transform.position, previewPlace.transform.rotation, previewPlace.transform);

        MeshRenderer renderer = previewModel.GetComponent<MeshRenderer>();
        renderer.material = materialPreview;

        currentModelID = modelID;
    }

    public void UnloadPreview()
    {
        if (previewModel != null)
        {
            Destroy(previewModel);
        }
    }

    public void DestroyModel(GameObject model, GameObject uiContainer)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == model)
            {
                Destroy(child.gameObject);
                Destroy(uiContainer.gameObject);
                break;
            }
        }
    }

    public void ChangeModel(int modelID)
    {
        UnloadPreview();
        EventManager.ScreenTapEvent();
        LoadPreview(modelID);
    }
}