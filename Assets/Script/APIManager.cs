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
        string url = "http://localhost/MYG/index.php?fullproductinfo=Double%20Bed";
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
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