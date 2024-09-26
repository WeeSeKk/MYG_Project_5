using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Globalization;
using TMPro;

public class APIManager : MonoBehaviour
{
    SpawnManagerScriptableObject spawnManagerScriptableObject;

    // Start is called before the first frame update
    void Start()
    {
        string productName = "Double%20Bed";
        string url = "http://localhost/MYG/index.php?fullproductinfo=" + productName;
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
                productInfo.prefabAdress = keys.GetValue("Prefab").ToString();

                SetupSO(productInfo);
            }
        }
        
    }

    void SetupSO(ProductInfo productInfo)
    {
        SpawnManagerScriptableObject newScriptableObject =  ScriptableObject.CreateInstance<SpawnManagerScriptableObject>();

        newScriptableObject.productID = productInfo.product_ID;
        newScriptableObject.modelID = productInfo.product_ID.ToString();
        newScriptableObject.productName = productInfo.product_Name;
        newScriptableObject.productPrice = productInfo.price;
        newScriptableObject.shortProductDescription = productInfo.short_Descritption;
        newScriptableObject.productDescription = productInfo.full_Descritption;
        newScriptableObject.productCategory = productInfo.tags;

        Debug.Log(productInfo.product_ID);
        Debug.Log(productInfo.product_Name);
        Debug.Log(productInfo.price);
        Debug.Log(productInfo.short_Descritption);
        Debug.Log(productInfo.full_Descritption);
        Debug.Log(productInfo.tags);
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
    public string prefabAdress { get; set; }
}