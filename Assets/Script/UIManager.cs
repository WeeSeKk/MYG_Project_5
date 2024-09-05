using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] VisualTreeAsset productTemplate;
    [SerializeField] VisualTreeAsset productTemplateAR;
    [SerializeField] VisualTreeAsset shippingTemplate;
    [SerializeField] VisualTreeAsset buyingOptionTemplate;
    [SerializeField] VisualTreeAsset listViewItem;
    [SerializeField] VisualTreeAsset endOfCategory;
    [SerializeField] VisualTreeAsset cartStartTemplate;
    [SerializeField] public List<CategoryScriptableObject> categorySO;
    List<SpawnManagerScriptableObject> scriptableObjects = new List<SpawnManagerScriptableObject>();
    List<string> categoryList = new List<string>();
    _3DModelsManager _3DModelsManager;
    ScrollView _0gridContainer;
    VisualElement loadingView;
    ScrollView productPage;
    ScrollView categoryScrollView;
    ScrollView cartScrollView;
    VisualElement productImage;
    VisualElement appView;
    VisualElement arView;
    VisualElement cartItemContainer;
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
    Button cartButton;
    Button registerButtonRegisterView;
    Button returnARButton;
    Button addToCartButton;
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
    VisualElement cartView;
    Label productNameInfoList;
    Label productDescriptionInfoList;
    Label productPriceInfo;
    TreeView categoryTreeView;
    ListView categoryListView;
    Button returnButtonCart;
    TextField firstNameTextfield;
    TextField lastNameTextfield;
    TextField birthdateTextfield;
    TextField adressTextfield;
    TextField zIPCodeTextfield;
    TextField cityTexfield;
    TextField phoneNumberTexfield;
    TextField emailTexfield;
    TextField passwordTexfield;
    TextField confirmPasswordTexfield;
    TextField emailTexfieldLogin;
    TextField passwordTexfieldLogin;
    Button loginButtonLoginView;
    bool productLoaded;
    string _productID;
    int modelID;
    int containerCategoryHeight;

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
        addToCartButton = root.Q<Button>("AddToCartButton");
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
        cartItemContainer = root.Q<VisualElement>("CartItemContainer");
        cartScrollView = root.Q<ScrollView>("CartScrollView");
        cartButton = root.Q<Button>("CartButton");
        cartView = root.Q<VisualElement>("CartView");
        registerButtonRegisterView = root.Q<Button>("RegisterButtonRegisterView");
        firstNameTextfield = root.Q<TextField>("FirstNameTextfield");
        lastNameTextfield = root.Q<TextField>("LastNameTextfield");
        birthdateTextfield = root.Q<TextField>("BirthdateTextfield");
        adressTextfield = root.Q<TextField>("AdressTextfield");
        zIPCodeTextfield = root.Q<TextField>("ZIPCodeTextfield");
        cityTexfield = root.Q<TextField>("CityTexfield");
        phoneNumberTexfield = root.Q<TextField>("PhoneNumberTexfield");
        emailTexfield = root.Q<TextField>("EmailTexfield");
        passwordTexfield = root.Q<TextField>("PasswordTexfield");
        confirmPasswordTexfield = root.Q<TextField>("ConfirmPasswordTexfield");
        loginButtonLoginView = root.Q<Button>("LoginButtonLoginView");
        emailTexfieldLogin = root.Q<TextField>("EmailTexfieldLogin");
        passwordTexfieldLogin = root.Q<TextField>("PasswordTexfieldLogin");

        AddTemplateToGrid();
        AddCategoryToList();
        SetupCartView();

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

        cartButton.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(ShowHideCartView(cartButton));
        });

        returnButtonCart.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(ShowHideCartView(returnButtonCart));
        });

        registerButtonRegisterView.RegisterCallback<ClickEvent>(evt =>
        {
            SetUserData();
        });

        loginButtonLoginView.RegisterCallback<ClickEvent>(evt =>
        {
            PlayfabManager.instance.OnLogin(emailTexfieldLogin.text, passwordTexfieldLogin.text);
        });
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))//test for debug
        {

        }
        if (Input.GetKeyDown("w"))//test for debug
        {
            if (cartScrollView.style.display == DisplayStyle.None)
            {
                cartScrollView.style.display = DisplayStyle.Flex;
            }
            else
            {
                cartScrollView.style.display = DisplayStyle.None;
            }
        }
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

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                if (scriptableObjects[i].modelID == productID.text)
                {
                    _3DModelsManager.ChangeModel(scriptableObjects[i].productID);
                    _productID = scriptableObjects[i].modelID;
                    UpdateARInfoButton();
                    break;
                }
            }
            ShowHideARList();
        }
        else if (button.name == "FavoriteButtonAR")
        {
            Debug.Log("Favorite Button AR");
        }
    }

    void AddTemplateToGrid()
    {
        int count = 0;

        while (count < AppManager.instance.scriptableObjects.Count)
        {
            VisualElement templateInstance = productTemplate.Instantiate();
            VisualElement templateInstanceAR = productTemplateAR.Instantiate();

            templateInstance.style.width = new Length(50, LengthUnit.Percent);
            templateInstance.style.height = new Length(40, LengthUnit.Percent);
            templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

            templateInstanceAR.style.width = new Length(100, LengthUnit.Percent);
            templateInstanceAR.style.height = new Length(30, LengthUnit.Percent);
            templateInstanceAR.transform.scale = new Vector3(1f, 1f, 1.0f);

            AddInfoToTemplate(templateInstance, templateInstanceAR);
            _0gridContainer.Add(templateInstance);
            scrollListAR.Add(templateInstanceAR);
            count++;

        }
        productLoaded = true;
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
            button.style.unityTextAlign = TextAnchor.MiddleLeft;
            Debug.Log(button.text);
            containerCategoryHeight += 100;

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
                button.style.unityTextAlign = TextAnchor.MiddleLeft;

                categoryScrollView.Add(templateInstance);
                containerCategoryHeight += 100;
            }

            templateInstance = endOfCategory.Instantiate();

            templateInstance.style.width = new Length(100, LengthUnit.Percent);
            templateInstance.style.height = new Length(2, LengthUnit.Percent);
            templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

            containerCategoryHeight += 50;

            categoryScrollView.Add(templateInstance);
        }
        SetupCategoryButtons();
    }

    void SetupCartView()
    {
        VisualElement templateInstance;
        int itemsInCart = 5;

        templateInstance = cartStartTemplate.Instantiate();
        templateInstance.style.width = new Length(100, LengthUnit.Percent);
        templateInstance.style.height = new Length(25, LengthUnit.Percent);
        templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

        cartScrollView.Add(templateInstance);

        returnButtonCart = root.Q<Button>("ReturnButtonCart");

        for (int a = 0; a < itemsInCart; a++)
        {
            cartScrollView.style.justifyContent = Justify.SpaceBetween;

            templateInstance = productTemplateAR.Instantiate();
            templateInstance.style.width = new Length(100, LengthUnit.Percent);
            templateInstance.style.height = new Length(15, LengthUnit.Percent);
            templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

            //AddInfoToTemplate(templateInstance);
            cartScrollView.Add(templateInstance);
        }

        templateInstance = shippingTemplate.Instantiate();
        templateInstance.style.width = new Length(100, LengthUnit.Percent);
        templateInstance.style.height = new Length(40, LengthUnit.Percent);
        templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

        cartScrollView.Add(templateInstance);

        templateInstance = buyingOptionTemplate.Instantiate();
        templateInstance.style.width = new Length(100, LengthUnit.Percent);
        templateInstance.style.height = new Length(50, LengthUnit.Percent);
        templateInstance.transform.scale = new Vector3(1f, 1f, 1.0f);

        cartScrollView.Add(templateInstance);


        cartScrollView.style.display = DisplayStyle.Flex;
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

    void AddInfoToTemplate(VisualElement template, VisualElement templateAR)
    {
        foreach (var scriptableObject in AppManager.instance.scriptableObjects)
        {
            if (!scriptableObjects.Contains(scriptableObject))
            {
                VisualElement productImage = template.Q<VisualElement>("ProductImage");
                productImage.style.backgroundImage = scriptableObject.modelImage;

                Label productID = template.Q<Label>("ProductID");
                productID.text = scriptableObject.modelID;

                Label productName = template.Q<Label>("ProductName");
                productName.text = scriptableObject.productName;

                Label productDescription = template.Q<Label>("ProductDescription");
                productDescription.text = scriptableObject.shortProductDescription;

                Label productPrice = template.Q<Label>("ProductPrice");
                productPrice.text = scriptableObject.productPrice.ToString() + " $";

                productImage = templateAR.Q<VisualElement>("ProductImage");
                productImage.style.backgroundImage = scriptableObject.modelImage;

                productID = templateAR.Q<Label>("ProductID");
                productID.text = scriptableObject.modelID;

                productName = templateAR.Q<Label>("ProductName");
                productName.text = scriptableObject.productName;

                productDescription = templateAR.Q<Label>("ProductDescription");
                productDescription.text = scriptableObject.shortProductDescription;

                productPrice = templateAR.Q<Label>("ProductPrice");
                productPrice.text = scriptableObject.productPrice.ToString() + " $";

                scriptableObjects.Add(scriptableObject);

                break;
            }
        }
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

    IEnumerator ShowHideCartView(Button button)
    {
        if (cartView.ClassListContains("LoginViewHidden"))
        {
            cartScrollView.style.display = DisplayStyle.Flex;
            ShowHideAccountInfo();
            cartView.RemoveFromClassList("LoginViewHidden");
        }
        else if (!cartView.ClassListContains("LoginViewHidden") && button == cartButton)
        {
            ShowHideAccountInfo();
        }
        else if (button != loginButton)
        {
            cartView.AddToClassList("LoginViewHidden");
            yield return new WaitForSeconds(1);
            cartScrollView.style.display = DisplayStyle.None;
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
            addToCartButton.AddToClassList("ButtonHidden2");
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
                productPrice.text = scriptableObjects[i].productPrice.ToString() + " $";
                productDescription.text = scriptableObjects[i].productDescription;
            }
        }
        productPage.AddToClassList("ProductPageVisible");
        buyButton.RemoveFromClassList("ButtonHidden2");
        addToCartButton.RemoveFromClassList("ButtonHidden2");
    }

    public void ChangeSceneUI(int scene)
    {
        if (scene == 1)
        {
            returnARButton.pickingMode = PickingMode.Position;
            appView.style.display = DisplayStyle.None;

            arView.style.display = DisplayStyle.Flex;

            modelButton.pickingMode = PickingMode.Position;
            SetupButtons(2);

            _3DModelsManager = GameObject.Find("3DModel").GetComponent<_3DModelsManager>();

            UpdateARInfoButton();
        }
        else
        {
            returnARButton.pickingMode = PickingMode.Ignore;
            appView.style.display = DisplayStyle.Flex;


            arView.style.display = DisplayStyle.None;

            modelButton.pickingMode = PickingMode.Ignore;
        }
    }

    void UpdateARInfoButton()
    {
        for (int i = 0; i < scriptableObjects.Count; i++)
        {
            if (scriptableObjects[i].modelID == _productID)
            {
                productInfoImage.style.backgroundImage = scriptableObjects[i].modelImage;
                productNameInfo.text = scriptableObjects[i].productName;
                productPriceInfo.text = scriptableObjects[i].productPrice.ToString();
                productDescriptionInfo.text = scriptableObjects[i].shortProductDescription;

                productInfoImageList.style.backgroundImage = scriptableObjects[i].modelImage;
                productDescriptionInfoList.text = scriptableObjects[i].shortProductDescription;
                productNameInfoList.text = scriptableObjects[i].productName;
                break;
            }
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

    void SetUserData()
    {
        TextField[] userDataTextFields = {firstNameTextfield, lastNameTextfield,
        birthdateTextfield, adressTextfield, zIPCodeTextfield, cityTexfield, phoneNumberTexfield,
        emailTexfield, passwordTexfield};

        if (passwordTexfield.text == confirmPasswordTexfield.text)
        {
            foreach (TextField textField in userDataTextFields)
            {
                if (textField.text == null)
                {
                    OnUserDataError("Missing info");
                }
                else
                {
                    UserData userData = new UserData();
                    userData.firstName = firstNameTextfield.text;
                    userData.lastName = lastNameTextfield.text;
                    userData.birthDate = birthdateTextfield.text;
                    userData.address = adressTextfield.text;
                    userData.zipCode = zIPCodeTextfield.text;
                    userData.city = cityTexfield.text;
                    userData.phoneNumber = phoneNumberTexfield.text;
                    userData.email = emailTexfield.text;
                    userData.password = passwordTexfield.text;

                    PlayfabManager.instance.OnRegister(userData);
                    break;
                }
            }
        }
        else
        {
            OnUserDataError("Passwords don't match");
        }
    }

    void OnUserDataError(string error)
    {
        Debug.LogError(error);
    }
}