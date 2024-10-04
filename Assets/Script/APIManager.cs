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

    void Start()
    {
        string urlGet = "http://localhost/MYG/index.php?fullproductinfo=Double%20Bed";
        StartCoroutine(GetRequest(urlGet));
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            JArray jArray = JArray.Parse(webRequest.downloadHandler.text);

            foreach (JObject keys in jArray)
            {
                ProductInfo productInfo = new ProductInfo();

                productInfo.product_ID = (int)keys.GetValue("Product_ID");
                productInfo.product_Name = keys.GetValue("Product_Name").ToString();
                productInfo.price = (int)keys.GetValue("Price");
                productInfo.short_Descritption = keys.GetValue("Short_Descritption").ToString();
                productInfo.full_Descritption = keys.GetValue("Full_Descritption").ToString();
                productInfo.tags = keys.GetValue("Tags").ToString();
                productInfo.prefabPath = keys.GetValue("Prefab").ToString();

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
            }
            else
            {
                //show error message
            }
        }
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
        uIManager.AddTemplateToGrid();
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