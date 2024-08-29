using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] VisualTreeAsset productTemplate;
    [SerializeField] VisualTreeAsset productTemplateAR;
    [SerializeField] VisualTreeAsset listViewItem;
    [SerializeField] VisualTreeAsset endOfCategory;
    [SerializeField] public List<SpawnManagerScriptableObject> scriptableObjects;
    [SerializeField] public List<CategoryScriptableObject> categorySO;
    List<string> categoryList = new List<string>();
    _3DModelsManager _3DModelsManager;
    ScrollView _0gridContainer;
    VisualElement loadingView;
    ScrollView productPage;
    ScrollView categoryScrollView;
    VisualElement productImage;
    VisualElement appView;
    VisualElement arView;
    Label productName;
    Label productPrice;
    Label productDescription;
    VisualElement root;
    VisualElement categoryView;
    ScrollView scrollListAR;
    VisualElement productListAR;
    VisualElement accountInfo;
    VisualElement loginView;
    VisualElement registerView;
    Button arButton;
    Button returnButton;
    Button returnButtonRegister;
    Button modelButton;
    Button productListInfoButton;
    Button returnARButton;
    Button cartButton;
    Button buyButton;
    Button categoryButton;
    Button closeARListButton;
    Button userButton;
    Button returnButtonLogin;
    Button loginButton;
    Button registerButton;
    VisualElement productInfoImage;
    Label productNameInfo;
    Label productDescriptionInfo;
    VisualElement productInfoImageList;
    Label productNameInfoList;
    Label productDescriptionInfoList;
    Label productPriceInfo;
    TreeView categoryTreeView;
    ListView categoryListView;
    bool productLoaded;
    string _productID;
    int modelID;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
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
        modelButton = root.Q<Button>("ModelButton");
        buyButton = root.Q<Button>("BuyButton");
        cartButton = root.Q<Button>("AddToCartButton");
        productInfoImage = root.Q<VisualElement>("ProductImageInfo");
        productNameInfo = root.Q<Label>("ProductNameInfo");
        productDescriptionInfo = root.Q<Label>("ProductDescriptionInfo");
        productPriceInfo = root.Q<Label>("ProductPriceInfo");
        categoryButton = root.Q<Button>("CategoryButton");
        categoryView = root.Q<VisualElement>("CategoryView");
        scrollListAR = root.Q<ScrollView>("ScrollViewAR");
        productListInfoButton = root.Q<Button>("ProductListInfoButton");
        productListAR = root.Q<VisualElement>("ProductListAR");
        closeARListButton = root.Q<Button>("CloseARListButton");
        productInfoImageList = root.Q<VisualElement>("ProductImageInfoList");
        productNameInfoList = root.Q<Label>("ProductNameInfoList");
        productDescriptionInfoList = root.Q<Label>("ProductDescriptionInfoList");
        userButton = root.Q<Button>("UserButton");
        accountInfo = root.Q<VisualElement>("AccountVisual");
        loginButton = root.Q<Button>("LoginButton");
        loginView = root.Q<VisualElement>("LoginView");
        returnButtonLogin = root.Q<Button>("ReturnButtonLogin");
        categoryTreeView = root.Q<TreeView>("CategoryTreeView");
        categoryListView = root.Q<ListView>("CategoryListView");
        categoryScrollView = root.Q<ScrollView>("CategoryScrollView");
        registerButton = root.Q<Button>("RegisterButton");
        registerView = root.Q<VisualElement>("RegisterView");
        returnButtonRegister = root.Q<Button>("ReturnButtonRegister");

        AddTemplateToGrid(_0gridContainer);
        AddCategoryToList();
        AddTemplateToGrid(scrollListAR);

        if (productLoaded)
        {
            SetupButtons(1);
        }

        arButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(AppManager.instance.LoadScenes(modelID));
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideProductPage("00");
        });

        returnARButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(AppManager.instance.LoadScenes(00));
        });

        modelButton.RegisterCallback<ClickEvent>(evt =>
        {
            NewContentPositioningBehaviour newContentPositioningBehaviour = GameObject.Find("Plane Finder").GetComponent<NewContentPositioningBehaviour>();
            _3DModelsManager _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();
            newContentPositioningBehaviour.HitTest();
            _3DModelsManager.UnloadPreview();
        });

        categoryButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideCategory();
        });

        productListInfoButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideARList();
        });

        closeARListButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideARList();
        });

        userButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideAccountInfo();
        });

        loginButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideLoginView(loginButton);
        });

        returnButtonLogin.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideLoginView(returnButtonLogin);
        });

        registerButton.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideRegisterView(loginButton);
        });

        returnButtonRegister.RegisterCallback<ClickEvent>(evt =>
        {
            ShowHideRegisterView(returnButtonRegister);
        });
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
        else if (button.name == "ProductButtonAR")
        {
            VisualElement template = button.parent;
            Label productID = template.Q<Label>("ProductID");

            ShowHideARList();

            _3DModelsManager.ChangeModel(productID.text);
        }
        else if (button.name == "FavoriteButtonAR")
        {
            Debug.Log("Favorite Button AR");
        }
    }

    void AddTemplateToGrid(ScrollView scrollView)
    {
        if (scrollView == _0gridContainer)
        {
            int count = 0;
            int maxCount = 20;

            scrollView.style.justifyContent = Justify.SpaceBetween;

            while (count < maxCount)
            {
                VisualElement templateInstance = productTemplate.Instantiate();

                templateInstance.style.width = new Length(50, LengthUnit.Percent);
                templateInstance.style.height = new Length(40, LengthUnit.Percent);
                templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

                AddInfoToTemplate(templateInstance);
                scrollView.Add(templateInstance);
                count++;

            }
            productLoaded = true;
        }
        else
        {
            int count = 0;
            int maxCount = 20;

            scrollView.style.justifyContent = Justify.SpaceBetween;

            while (count < maxCount)
            {
                VisualElement templateInstance = productTemplateAR.Instantiate();

                templateInstance.style.width = new Length(100, LengthUnit.Percent);
                templateInstance.style.height = new Length(30, LengthUnit.Percent);
                templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

                AddInfoToTemplate(templateInstance);
                scrollView.Add(templateInstance);
                count++;
            }
        }
    }

    void AddCategoryToList()
    {
        for (int i = 0; i < categorySO.Count; i++)
        {
            VisualElement templateInstance = listViewItem.Instantiate();

            templateInstance.style.width = new Length(100, LengthUnit.Percent);
            templateInstance.style.height = new Length(5, LengthUnit.Percent);
            templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

            Button button = templateInstance.Q<Button>("CategoryItemButton");
            button.text = categorySO[i].categoryHeader + " :";
            button.style.fontSize = 70;
            Debug.Log(button.text);

            categoryScrollView.Add(templateInstance);

            for (int a = 0; a < categorySO[i].categoryItems.Count; a++)
            {
                templateInstance = listViewItem.Instantiate();

                templateInstance.style.width = new Length(100, LengthUnit.Percent);
                templateInstance.style.height = new Length(5, LengthUnit.Percent);
                templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

                button = templateInstance.Q<Button>("CategoryItemButton");
                button.text = categorySO[i].categoryItems[a];
                button.style.fontSize = 50;

                categoryScrollView.Add(templateInstance);
            }

            templateInstance = endOfCategory.Instantiate();

            templateInstance.style.width = new Length(100, LengthUnit.Percent);
            templateInstance.style.height = new Length(3, LengthUnit.Percent);
            templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

            categoryScrollView.Add(templateInstance);
        }
        SetupCategoryButtons();
    }

    void SetupButtons(int scene)
    {
        if (scene == 1)
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
        else
        {
            var productButtonsAR = root.Query<Button>(className: "ProductButtonAR").ToList();
            var favoriteButtonAR = root.Query<Button>(className: "FavoriteButtonAR").ToList();

            foreach (var button in productButtonsAR)
            {
                button.clicked += () => OnButtonClick(button);
            }
            foreach (var button in favoriteButtonAR)
            {
                button.clicked += () => OnButtonClick(button);
            }
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

    void ShowHideCategory()
    {
        if (categoryView.ClassListContains("CategoryViewHidden"))
        {
            categoryView.RemoveFromClassList("CategoryViewHidden");
        }
        else
        {
            categoryView.AddToClassList("CategoryViewHidden");
        }
    }

    void ShowHideARList()
    {
        if (productListAR.ClassListContains("ProductListHidden"))
        {
            productListAR.RemoveFromClassList("ProductListHidden");
        }
        else
        {
            productListAR.AddToClassList("ProductListHidden");
        }
    }

    void ShowHideAccountInfo()
    {
        if (accountInfo.ClassListContains("AccountVisualHidden"))
        {
            accountInfo.RemoveFromClassList("AccountVisualHidden");
        }
        else
        {
            accountInfo.AddToClassList("AccountVisualHidden");
        }
    }

    void ShowHideLoginView(Button button)
    {
        if (loginView.ClassListContains("LoginViewHidden"))
        {
            loginView.RemoveFromClassList("LoginViewHidden");
            ShowHideAccountInfo();
        }
        else if (!loginView.ClassListContains("LoginViewHidden") && button == loginButton)
        {
            ShowHideAccountInfo();
        }
        else if (button != loginButton)
        {
            loginView.AddToClassList("LoginViewHidden");
        }
    }

    void ShowHideRegisterView(Button button)
    {
        if (registerView.ClassListContains("LoginViewHidden"))
        {
            registerView.RemoveFromClassList("LoginViewHidden");
            ShowHideAccountInfo();
        }
        else if (!registerView.ClassListContains("LoginViewHidden") && button == registerButton)
        {
            ShowHideAccountInfo();
        }
        else if (button != loginButton)
        {
            registerView.AddToClassList("LoginViewHidden");
        }
    }

    public void LoadingScreen()
    {
        if (loadingView.ClassListContains("LoadingViewHidden"))
        {
            loadingView.RemoveFromClassList("LoadingViewHidden");
        }
        else
        {
            loadingView.AddToClassList("LoadingViewHidden");
        }
    }

    void ShowHideProductPage(string productID)
    {
        if (productID == "00")
        {
            productPage.RemoveFromClassList("ProductPageVisible");
            buyButton.AddToClassList("ButtonHidden2");
            cartButton.AddToClassList("ButtonHidden2");
            return;
        }
        for (int i = 0; i < scriptableObjects.Count; i++)
        {
            if (scriptableObjects[i].modelID == productID)
            {
                modelID = scriptableObjects[i].productID;
                _productID = scriptableObjects[i].modelID;
                productImage.style.backgroundImage = scriptableObjects[i].modelImage;
                productName.text = scriptableObjects[i].productName;
                productPrice.text = scriptableObjects[i].productPrice.ToString();
                productDescription.text = scriptableObjects[i].productDescription;
            }
        }
        productPage.AddToClassList("ProductPageVisible");
        buyButton.RemoveFromClassList("ButtonHidden2");
        cartButton.RemoveFromClassList("ButtonHidden2");
    }

    public void ChangeSceneUI(int scene)
    {
        if (scene == 1)
        {
            returnARButton.pickingMode = PickingMode.Position;
            appView.style.display = DisplayStyle.None;

            arView.style.display = DisplayStyle.Flex;

            modelButton.pickingMode = PickingMode.Position;
            AddTemplateToGrid(scrollListAR);
            SetupButtons(2);

            _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                if (scriptableObjects[i].modelID == _productID)
                {
                    productInfoImage.style.backgroundImage = scriptableObjects[i].modelImage;
                    productNameInfo.text = scriptableObjects[i].productName;
                    productPriceInfo.text = scriptableObjects[i].productPrice.ToString();
                    productDescriptionInfo.text = scriptableObjects[i].productDescription;

                    productInfoImageList.style.backgroundImage = scriptableObjects[i].modelImage;
                    productDescriptionInfoList.text = scriptableObjects[i].productDescription;
                    productNameInfoList.text = scriptableObjects[i].productName;
                    break;
                }
            }
        }
        else
        {
            returnARButton.pickingMode = PickingMode.Ignore;
            appView.style.display = DisplayStyle.Flex;


            arView.style.display = DisplayStyle.None;

            modelButton.pickingMode = PickingMode.Ignore;
        }
    }

    void SetupCategoryButtons()
    {
        var categoryButtons = root.Query<Button>(className: "CategoryItemButton").ToList();

        foreach (var button in categoryButtons)
        {
            button.clicked += () => OnCategoryButtonClicked(button);
        }
    }

    void OnCategoryButtonClicked(Button button)
    {
        Debug.Log(button.text);
    }
}