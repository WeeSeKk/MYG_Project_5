using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] VisualTreeAsset productTemplate;
    [SerializeField] public List<SpawnManagerScriptableObject> scriptableObjects;
    ScrollView _0gridContainer;
    VisualElement gridContainer;
    VisualElement loadingView;
    ScrollView productPage;
    VisualElement productImage;
    VisualElement appView;
    VisualElement arView;
    Label productName;
    Label productPrice;
    Label productDescription;
    VisualElement root;
    Button arButton;
    Button returnButton;
    Button returnARButton;
    bool productLoaded;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        gridContainer = root.Q<VisualElement>("GridContainer");
        _0gridContainer = root.Q<ScrollView>("0GridContainer");
        productPage = root.Q<ScrollView>("ProductPage");
        productImage = root.Q<VisualElement>("ProductImage");
        productName = root.Q<Label>("ProductName");
        productPrice = root.Q<Label>("ProductPrice");
        productDescription = root.Q<Label>("ProductDescription");
        arButton = root.Q<Button>("ARButton");
        appView = root.Q<VisualElement>("AppView");
        arView = root.Q<VisualElement>("ARView");
        returnButton = root.Q<Button>("ReturnButton");
        returnARButton = root.Q<Button>("ReturnARButton");
        loadingView = root.Q<VisualElement>("LoadingView");


        AddTemplateToGrid(productTemplate);

        if (productLoaded)
        {
            SetupButtons();
        }

        arButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(LoadARScene());
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideProductPage("00");
        });

        returnARButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(LoadARScene());
        });

        arView.pickingMode = PickingMode.Ignore;
    }

    void OnButtonClick(Button button)
    {
        if (button.name == "ProductButton")
        {
            VisualElement template = button.parent;
            Label productID = template.Q<Label>("ProductID");

            ShowHideProductPage(productID.text);

            Debug.Log("Product Button");
        }
        else if (button.name == "FavoriteButton")
        {
            Debug.Log("Favorite Button");
        }
    }

    void AddTemplateToGrid(VisualTreeAsset template)
    {
        int count = 0;
        int maxCount = 17;

        _0gridContainer.style.justifyContent = Justify.SpaceBetween;

        while (count < maxCount)
        {
            VisualElement templateInstance = template.Instantiate();

            templateInstance.style.width = new Length(50, LengthUnit.Percent);
            templateInstance.style.height = new Length(40, LengthUnit.Percent);
            templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

            AddInfoToTemplate(templateInstance);
            _0gridContainer.Add(templateInstance);
            count++;

        }
        productLoaded = true;
    }

    void SetupButtons()
    {
        var productButtons = root.Query<Button>(className: "ProductButton").ToList();
        var favoriteButton = root.Query<Button>(className: "FavoriteButton").ToList();

        foreach (var button in productButtons)
        {
            button.clicked += () => OnButtonClick(button);
        }
        foreach (var button in favoriteButton)
        {
            button.clicked += () => OnButtonClick(button);
        }
    }

    void AddInfoToTemplate(VisualElement template)
    {
        VisualElement productImage = template.Q<VisualElement>("ProductImage");
        productImage.style.backgroundImage = scriptableObjects[0].modelImage;

        Label productID = template.Q<Label>("ProductID");
        productID.text = scriptableObjects[0].modelID;

        Label productName = template.Q<Label>("ProductName");
        productName.text = scriptableObjects[0].productName;

        Label productDescription = template.Q<Label>("ProductDescription");
        productDescription.text = scriptableObjects[0].productDescription;

        Label productPrice = template.Q<Label>("ProductPrice");
        productPrice.text = scriptableObjects[0].productPrice.ToString();
    }

    void ShowHideProductPage(string productID)
    {
        if (productID == "00")
        {
            productPage.RemoveFromClassList("ProductPageVisible");
            return;
        }
        for (int i = 0; i < scriptableObjects.Count; i++)
        {
            if (scriptableObjects[i].modelID == productID)
            {
                productImage.style.backgroundImage = scriptableObjects[i].modelImage;
                productName.text = scriptableObjects[i].productName;
                productPrice.text = scriptableObjects[i].productPrice.ToString();
                productDescription.text = scriptableObjects[i].productDescription;
            }
        }
        productPage.AddToClassList("ProductPageVisible");
    }

    IEnumerator LoadARScene()
    {
        if (SceneManager.loadedSceneCount <= 1)
        {
            appView.style.opacity = 0;
            appView.visible = false;
            appView.pickingMode = PickingMode.Ignore;

            arView.style.opacity = 100;
            arView.pickingMode = PickingMode.Ignore;

            loadingView.RemoveFromClassList("LoadingViewHidden");
            yield return SceneManager.LoadSceneAsync("ARScene", LoadSceneMode.Additive);
            loadingView.AddToClassList("LoadingViewHidden");
        }
        else
        {
            appView.style.opacity = 100;
            appView.visible = true;
            appView.pickingMode = PickingMode.Ignore;

            arView.style.opacity = 0;
            arView.pickingMode = PickingMode.Ignore;

            loadingView.RemoveFromClassList("LoadingViewHidden");
            yield return SceneManager.UnloadSceneAsync("ARScene");
            loadingView.AddToClassList("LoadingViewHidden");
        }
    }
}
