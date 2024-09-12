using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using System.Threading.Tasks;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    [SerializeField] UIManager uIManager;
    List<int> cartInfo = new List<int>();
    UserData _userData;
    bool register;
    public bool loggedIn;
    public string cartInfoString;

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
        if (Input.GetKeyDown("space"))//test for debug
        {

        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void OnRegister(UserData userData)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = userData.email,
            Password = userData.password,
            RequireBothUsernameAndEmail = false
        };
        _userData = userData;
        register = true;
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucces, OnError);
    }

    void SaveUserInfo(UserData userData)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"FirstName", userData.firstName},
                {"LastName", userData.lastName},
                {"BirthDate", userData.birthDate},
                {"Address", userData.address},
                {"ZIPCode", userData.zipCode},
                {"City", userData.city},
                {"PhoneNumber", userData.phoneNumber}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnUserInfoSend, OnError);
    }

    void OnUserInfoSend(UpdateUserDataResult result)
    {
        Debug.Log("Data send");
    }

    void OnRegisterSucces(RegisterPlayFabUserResult result)
    {
        OnLogin(_userData.email, _userData.password);
    }

    public void OnLogin(string email, string password)
    {
        if (_userData == null)
        {
            _userData = new UserData();
        }
        _userData.email = email;

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucces, OnError);
        loggedIn = true;
    }

    void OnLoginSucces(LoginResult result)
    {
        if (register)
        {
            SaveUserInfo(_userData);
            register = false;
        }
        Debug.Log("Logged In");
        uIManager.ShowHideLoginInfo(_userData.email);
        OnLoadingCart();
        GetUserInfo();
    }

    public async Task OnAddedToCart(string modelID)
    {
        await OnLoadingCart();

        cartInfoString = null;

        foreach (int num in cartInfo)
        {
            cartInfoString += num.ToString();
            cartInfoString += ",";
        }

        cartInfoString += modelID;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Cart", cartInfoString}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnAddedToCartSucces, OnError);
    }

    void OnAddedToCartSucces(UpdateUserDataResult result)
    {
        Debug.Log(result);
        uIManager.SetupCartView(cartInfoString);
    }

    public Task OnLoadingCart()
    {
        var tsk = new TaskCompletionSource<bool>();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), (result) =>
        {
            OnCartLoaded(result);
            tsk.SetResult(true);
        }, (error) =>
        {
            OnError(error);
            tsk.SetResult(false);
        });

        return tsk.Task;
    }

    void OnCartLoaded(GetUserDataResult result)
    {
        cartInfo.Clear();

        if (result.Data != null && result.Data.ContainsKey("Cart"))
        {
            string[] stringArray = result.Data["Cart"].Value.Split(',');

            foreach (string number in stringArray)
            {
                cartInfo.Add(int.Parse(number));
            }

            foreach (int num in cartInfo)
            {
                cartInfoString += num.ToString();
                cartInfoString += ",";
            }

            string cartInfostring2 = cartInfoString.Substring(0, cartInfoString.Length - 1);

            uIManager.SetupCartView(cartInfostring2);
        }
    }

    public void UpdateCartInfo(string cartItems)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Cart", cartItems}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnUpdatedCartSucces, OnError);
    }

    void OnUpdatedCartSucces(UpdateUserDataResult result)
    {
        Debug.Log("Cart Updated");
    }

    public void GetUserInfo()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetUserInfo, OnError);
    }

    void OnGetUserInfo(GetUserDataResult result)
    {
        UserData userData = new UserData();
        
        userData.firstName = result.Data["FirstName"].Value;
        userData.lastName = result.Data["LastName"].Value;
        userData.birthDate = result.Data["BirthDate"].Value;
        userData.address = result.Data["Address"].Value;
        userData.zipCode = result.Data["ZIPCode"].Value;
        userData.city = result.Data["City"].Value;
        userData.phoneNumber = result.Data["PhoneNumber"].Value;

        uIManager.SetupAccountInfos(userData);
    }

    public void UpdateUserData(UserData userData)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"FirstName", userData.firstName},
                {"LastName", userData.lastName},
                {"BirthDate", userData.birthDate},
                {"Address", userData.address},
                {"ZIPCode", userData.zipCode},
                {"City", userData.city},
                {"PhoneNumber", userData.phoneNumber}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnUpdatedUserData, OnError);
    }

    void OnUpdatedUserData(UpdateUserDataResult result)
    {
        Debug.Log("User Info Updated");
    }
}

public class UserData
{
    public string email { get; set; }
    public string password { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string birthDate { get; set; }
    public string address { get; set; }
    public string zipCode { get; set; }
    public string city { get; set; }
    public string phoneNumber { get; set; }
}