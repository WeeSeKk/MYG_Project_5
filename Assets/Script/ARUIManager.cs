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
    Quaternion originalRotation;
    Image modelInfo;
    LeanDragTranslate leanDragTranslate;
    LeanTwistRotateAxis leanTwistRotateAxis;
    bool showInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        leanTwistRotateAxis = linkedGameobject.GetComponent<LeanTwistRotateAxis>();

        leanDragTranslate.enabled = false;
        leanTwistRotateAxis.enabled = false;

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

    // Update is called once per frame
    void Update()
    {
        if (linkedGameobject != null)
        {
            this.transform.position = linkedGameobject.transform.position;
        }

        //transform.rotation = _camera.transform.rotation * originalRotation;

    }

    public void DeleteModel()
    {
        _3DModelsManager.DestroyModel(linkedGameobject, this.gameObject);
    }

    public void RotateModel()
    {
        if (leanTwistRotateAxis.enabled == false)
        {
            deleteButton.gameObject.SetActive(false);
            dragButton.gameObject.SetActive(false);
            cartButton.gameObject.SetActive(false);
            infoButton.gameObject.SetActive(false);

            leanTwistRotateAxis.enabled = true;
        }
        else
        {
            deleteButton.gameObject.SetActive(true);
            dragButton.gameObject.SetActive(true);
            cartButton.gameObject.SetActive(true);
            infoButton.gameObject.SetActive(true);

            leanTwistRotateAxis.enabled = false;
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
        /*if (linkedGameobject == null)
        {
            linkedGameobject = gameObject.transform.parent.gameObject;
            Debug.Log("linkedGameobject");
            return;
        }*/

        GameObject model = linkedGameobject.transform.GetChild(0).gameObject;

        if (model == gameObject)
        {
            if (container.activeSelf == false)
            {
                container.SetActive(true);
            }
            else
            {
                container.SetActive(false);
            }
        }
    }

    public void AddModelToCart()
    {
        //add product to cart
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
        rotateButton.gameObject.SetActive(true);
        deleteButton.gameObject.SetActive(true);
        dragButton.gameObject.SetActive(true);
        cartButton.gameObject.SetActive(true);
        infoButton.gameObject.SetActive(true);

        leanDragTranslate.enabled = false;
        leanTwistRotateAxis.enabled = false;

        if (container.activeSelf == true)
        {
            container.SetActive(false);
        }
    }
}
