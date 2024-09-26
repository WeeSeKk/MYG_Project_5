using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;

public class ARUIManager : MonoBehaviour
{
    [SerializeField] Button deleteButton;
    [SerializeField] Button dragButton;
    [SerializeField] Button rotateButton;
    [SerializeField] Button cartButton;
    [SerializeField] Button infoButton;
    [SerializeField] GameObject container;
    [SerializeField] Camera _camera;
    _3DModelsManager _3DModelsManager;
    GameObject linkedGameobject;
    ModelManager modelManager;
    Quaternion originalRotation;
    Image modelInfo;
    LeanDragTranslate leanDragTranslate;
    bool showInfo;

    void OnEnable()
    {
        EventManager.showHideInfo += ShowHideModelUI;
        EventManager.screenTap += ScreenTap;

        _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();
        _camera = GameObject.Find("ARCamera").GetComponent<Camera>();

        linkedGameobject = _3DModelsManager.Model();
        modelInfo = _3DModelsManager.modelInfoImage();

        container.SetActive(false);

        leanDragTranslate = linkedGameobject.GetComponent<LeanDragTranslate>();

        leanDragTranslate.enabled = false;

        originalRotation = transform.rotation;
    }

    void OnDisable()
    {
        EventManager.showHideInfo -= ShowHideModelUI;
        EventManager.screenTap -= ScreenTap;

        deleteButton.gameObject.SetActive(false);
        dragButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);
        cartButton.gameObject.SetActive(false);
        infoButton.gameObject.SetActive(false);
        linkedGameobject = null;
        modelInfo = null;
    }

    void Update()
    {
        if (linkedGameobject != null)
        {
            this.transform.position = linkedGameobject.transform.position;
        }

        transform.rotation = _camera.transform.rotation * originalRotation;
    }

    public void DeleteModel()
    {
        _3DModelsManager.DestroyModel(linkedGameobject, this.gameObject);
    }

    public void RotateModel()
    {
        modelManager = linkedGameobject.GetComponent<ModelManager>();

        if (modelManager.rotate == false)
        {
            deleteButton.gameObject.SetActive(false);
            dragButton.gameObject.SetActive(false);
            cartButton.gameObject.SetActive(false);
            infoButton.gameObject.SetActive(false);

            modelManager.rotate = true;
        }
        else
        {
            deleteButton.gameObject.SetActive(true);
            dragButton.gameObject.SetActive(true);
            cartButton.gameObject.SetActive(true);
            infoButton.gameObject.SetActive(true);

            modelManager.rotate = false;
        }
    }

    public void DragModel()
    {
        if (leanDragTranslate.enabled == false)
        {
            rotateButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
            cartButton.gameObject.SetActive(false);
            infoButton.gameObject.SetActive(false);

            leanDragTranslate.enabled = true;
            leanDragTranslate.Camera = _camera;
        }
        else
        {
            rotateButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
            cartButton.gameObject.SetActive(true);
            infoButton.gameObject.SetActive(true);

            leanDragTranslate.enabled = false;
        }
    }

    public void ShowHideModelUI(GameObject gameObject)
    {
        GameObject model = linkedGameobject.transform.GetChild(0).gameObject;

        if (model == gameObject)
        {
            if (container.activeSelf == false)
            {
                container.SetActive(true);
            }
            else
            {
                ScreenTap();
                container.SetActive(false);
            }
        }
    }

    public void AddModelToCart()
    {
        //PlayfabManager.instance.OnAddedToCart(_productID);
    }

    public void ShowHideModelInfo()
    {
        modelInfo = this.transform.GetChild(1).gameObject.GetComponent<Image>();
        modelInfo.transform.position = new Vector3(linkedGameobject.transform.position.x, modelInfo.transform.position.y, linkedGameobject.transform.position.z);

        if (showInfo == false)
        {
            rotateButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
            dragButton.gameObject.SetActive(false);
            cartButton.gameObject.SetActive(false);

            modelInfo.gameObject.SetActive(true);
            showInfo = true;
        }
        else
        {
            rotateButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
            dragButton.gameObject.SetActive(true);
            cartButton.gameObject.SetActive(true);

            modelInfo.gameObject.SetActive(false);
            showInfo = false;
        }
    }

    void ScreenTap()
    {
        modelInfo = this.transform.GetChild(1).gameObject.GetComponent<Image>();
        modelInfo.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(true);
        deleteButton.gameObject.SetActive(true);
        dragButton.gameObject.SetActive(true);
        cartButton.gameObject.SetActive(true);
        infoButton.gameObject.SetActive(true);

        leanDragTranslate.enabled = false;
        
        if (modelManager != null)
        {
            modelManager.rotate = false;
        }
        

        if (container.activeSelf == true)
        {
            container.SetActive(false);
        }
    }
}