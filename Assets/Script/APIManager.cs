using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class APIManager : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    public static APIManager instance;
    public List<SpawnManagerScriptableObject> scriptableObjects;
    public List<int> cartInfo = new List<int>();
    public bool loggedIn;

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

    async void Start()
    {
        string urlGet = "http://localhost/MYG/index.php?fullproductinfo=Double%20Bed";
        await GetRequestAsync(urlGet);
        uIManager.AddTemplateToGrid();
    }

    public async Task GetRequestAsync(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    return; // Sortir si erreur
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    return; // Sortir si erreur
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            JArray jArray = JArray.Parse(webRequest.downloadHandler.text);

            foreach (JObject keys in jArray)
            {
                ProductInfo productInfo = new ProductInfo
                {
                    product_ID = (int)keys.GetValue("Product_ID"),
                    product_Name = keys.GetValue("Product_Name").ToString(),
                    price = (int)keys.GetValue("Price"),
                    short_Descritption = keys.GetValue("Short_Descritption").ToString(),
                    full_Descritption = keys.GetValue("Full_Descritption").ToString(),
                    tags = keys.GetValue("Tags").ToString(),
                    prefabPath = keys.GetValue("Prefab").ToString()
                };

                SetupSO(productInfo);
            }
        }
    }

    public IEnumerator SendRequest(ClientData clientData, string action)
    {
        if (action == "register")
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("action", "register"));
            formData.Add(new MultipartFormDataSection("first_name", clientData.first_name));
            formData.Add(new MultipartFormDataSection("last_name", clientData.last_name));
            formData.Add(new MultipartFormDataSection("email", clientData.email));
            formData.Add(new MultipartFormDataSection("password", clientData.password));
            formData.Add(new MultipartFormDataSection("address", clientData.address));
            formData.Add(new MultipartFormDataSection("zip_code", clientData.zip_code));
            formData.Add(new MultipartFormDataSection("city", clientData.city));
            formData.Add(new MultipartFormDataSection("birthdate", clientData.birthdate));
            formData.Add(new MultipartFormDataSection("phone_number", clientData.phone_number));

            UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }

            JObject jsonResponse = JObject.Parse(www.downloadHandler.text);
            string resultString = "";
            foreach (var key in jsonResponse)
            {
                resultString += $"{key.Key}: {key.Value}\n";
            }

            Debug.Log(resultString);

            //if register succes then login 

            if (resultString.Contains("Success: True"))
            {

                StartCoroutine(SendRequest(clientData, "login"));
            }
            else
            {
                //show error message
            }
        }
        else if (action == "login")
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("action", "login"));
            formData.Add(new MultipartFormDataSection("email", clientData.email));
            formData.Add(new MultipartFormDataSection("password", clientData.password));

            Debug.Log(clientData.email + " " + clientData.password);

            UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
            yield return www.SendWebRequest();

            JObject jsonResponse = JObject.Parse(www.downloadHandler.text);
            string resultString = "";
            foreach (var key in jsonResponse)
            {
                resultString += $"{key.Key}: {key.Value}\n";
            }

            Debug.Log(resultString);

            if (resultString.Contains("Success: True"))
            {
                uIManager.ShowHideLoginInfo(clientData.email);
                StartCoroutine(GetCartContent(clientData.email));
                StartCoroutine(GetUserInfos(clientData.email));
                loggedIn = true;

                if (resultString.Contains("Role: admin"))
                {
                    Debug.Log("This Account is an admin");
                    uIManager.ShowAdminView(true);
                }
                else
                {
                    Debug.Log("This Account is not an admin");
                    uIManager.ShowAdminView(false);
                }
            }
            else
            {
                string errorMessage = (string)jsonResponse["Error Message"];
                StartCoroutine(uIManager.ShowHideErrorMessages(errorMessage));
                //show error message
            }
        }
    }

    public IEnumerator GetUserInfos(string email)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("action", "getaccountinfo"));
        formData.Add(new MultipartFormDataSection("email", email));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
        yield return www.SendWebRequest();

        JObject jsonResponse = JObject.Parse(www.downloadHandler.text);
        JObject userInfo = (JObject)jsonResponse["UserInfo"];

        ClientData clientData = new ClientData
        {
            first_name = (string)userInfo.GetValue("FirstName"),
            last_name = (string)userInfo.GetValue("LastName"),
            email = (string)userInfo.GetValue("Email"),
            password = (string)userInfo.GetValue("Password"),
            address = (string)userInfo.GetValue("Address"),
            zip_code = (string)userInfo.GetValue("ZIP_Code"),
            city = (string)userInfo.GetValue("City"),
            birthdate = (string)userInfo.GetValue("Birthdate"),
            phone_number = (string)userInfo.GetValue("Phone_Number")
        };
        uIManager.SetupAccountInfos(clientData);
        Debug.Log((string)userInfo.GetValue("FirstName"));
    }

    public IEnumerator UpdateUserInfos(ClientData clientData, string email)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("action", "updateaccountinfo"));
        formData.Add(new MultipartFormDataSection("first_name", clientData.first_name));
        formData.Add(new MultipartFormDataSection("last_name", clientData.last_name));
        formData.Add(new MultipartFormDataSection("address", clientData.address));
        formData.Add(new MultipartFormDataSection("zip_code", clientData.zip_code));
        formData.Add(new MultipartFormDataSection("city", clientData.city));
        formData.Add(new MultipartFormDataSection("birthdate", clientData.birthdate));
        formData.Add(new MultipartFormDataSection("phone_number", clientData.phone_number));
        formData.Add(new MultipartFormDataSection("email", email));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
        yield return www.SendWebRequest();

        //JObject jsonResponse = JObject.Parse(www.downloadHandler.text);

        Debug.Log(www.downloadHandler.text);
    }

    public IEnumerator EditProductInfo(string productName, string productDescription, string productPrice, int productID)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("action", "editproduct"));
        formData.Add(new MultipartFormDataSection("productName", productName));
        formData.Add(new MultipartFormDataSection("productDescription", productDescription));
        formData.Add(new MultipartFormDataSection("productPrice", productPrice));
        formData.Add(new MultipartFormDataSection("productID", productID.ToString()));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
        /*
        JObject jsonResponse = JObject.Parse(www.downloadHandler.text);
        string resultString = "";
        foreach (var key in jsonResponse)
        {
            resultString += $"{key.Key}: {key.Value}\n";
        }

        Debug.Log(resultString);*/

    }

    public IEnumerator GetCartContent(string email)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("action", "getCartContent"));
        formData.Add(new MultipartFormDataSection("email", email));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
        yield return www.SendWebRequest();

        JObject jsonResponse = JObject.Parse(www.downloadHandler.text);

        string itemsIdString = (string)jsonResponse["CartContent"];

        string[] stringArray = itemsIdString.Split(',');

        foreach (string number in stringArray)
        {
            cartInfo.Add(int.Parse(number));
        }

        uIManager.SetupCartView(itemsIdString);
    }

    public IEnumerator UpdateCart(string modelID, string email, bool remove)
    {
        if (!remove)
        {
            string cartInfoString = "";

            foreach (int num in cartInfo)
            {
                cartInfoString += num.ToString();
                cartInfoString += ",";
            }

            cartInfoString += modelID;
            cartInfo.Add(int.Parse(modelID));


            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("action", "updateCart"));
            formData.Add(new MultipartFormDataSection("email", email));
            formData.Add(new MultipartFormDataSection("cart_content", cartInfoString));

            UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
            yield return www.SendWebRequest();

            Debug.Log(www.downloadHandler.text);

            uIManager.SetupCartView(cartInfoString);
        }
        else
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("action", "updateCart"));
            formData.Add(new MultipartFormDataSection("email", email));
            formData.Add(new MultipartFormDataSection("cart_content", modelID));

            UnityWebRequest www = UnityWebRequest.Post("http://localhost/MYG/insert.php", formData);
            yield return www.SendWebRequest();

            Debug.Log(www.downloadHandler.text);
        }
    }


    void SetupSO(ProductInfo productInfo)
    {
        SpawnManagerScriptableObject newScriptableObject = ScriptableObject.CreateInstance<SpawnManagerScriptableObject>();

        newScriptableObject.productID = productInfo.product_ID;
        newScriptableObject.modelID = productInfo.product_ID.ToString();
        newScriptableObject.productName = productInfo.product_Name;
        newScriptableObject.productPrice = productInfo.price;
        newScriptableObject.shortProductDescription = productInfo.short_Descritption;
        newScriptableObject.productDescription = productInfo.full_Descritption;
        newScriptableObject.productCategory = productInfo.tags;
        newScriptableObject.modelImage = Resources.Load<Texture2D>("ProductImages/" + productInfo.product_Name);
        Addressables.LoadAssetAsync<GameObject>(productInfo.prefabPath);

        scriptableObjects.Add(newScriptableObject);
        //uIManager.AddTemplateToGrid();
    }

    public void Disconnect()
    {
        loggedIn = false;
        uIManager.ShowHideLoginInfo(null);
    }
}

public class ProductInfo
{
    public int product_ID { get; set; }
    public string product_Name { get; set; }
    public int price { get; set; }
    public string short_Descritption { get; set; }
    public string full_Descritption { get; set; }
    public string tags { get; set; }
    public string prefabPath { get; set; }
}

public class ClientData
{
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string address { get; set; }
    public string zip_code { get; set; }
    public string city { get; set; }
    public string birthdate { get; set; }
    public string phone_number { get; set; }
}