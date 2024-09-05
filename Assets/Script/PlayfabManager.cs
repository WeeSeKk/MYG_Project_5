using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    UserData _userData;
    bool register;

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
        var request = new UpdateUserDataRequest {
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
    }

    void OnLoginSucces(LoginResult result)
    {
        if (register)
        {
            SaveUserInfo(_userData);
            register = false;
        }
        Debug.Log("Logged In");
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