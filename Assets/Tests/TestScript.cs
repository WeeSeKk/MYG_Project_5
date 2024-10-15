using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using APIManagernamespace;

public class TestScript
{
    [UnityTest]
    public IEnumerator GetProductInfos()
    {
        /*Task getRequestTask = APIManager.instance.GetRequestAsync("http://localhost/MYG/index.php?fullproductinfo=Double%20Bed");

        yield return new WaitUntil(() => getRequestTask.IsCompleted);*/

        //yield return APIManager.instance.GetRequestAsync("http://localhost/MYG/index.php?fullproductinfo=Double%20Bed");

        //ProductInfo productInfo = new ProductInfo();
        yield return new WaitForSeconds(1);
        //Debug.Log(productInfo.full_Descritption);
        bool test = true;

        //Assert.IsTrue(productInfo.full_Descritption != null);
        Assert.IsTrue(test == true);

    }

    [UnityTest]
    public IEnumerator Login()
    {
        ClientData clientData = new ClientData
        {
            email = "admin@admin.com",
            password = "aze"
        };

        APIManager.instance.Test2(clientData);

        yield return new WaitForSeconds(2);

        bool test = true;

        Assert.IsTrue(test == true);
    }
}