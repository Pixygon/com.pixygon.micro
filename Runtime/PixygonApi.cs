using System;
using System.Threading.Tasks;
using Pixygon.Saving;
using UnityEngine;
using UnityEngine.Networking;

public class PixygonApi : MonoBehaviour {
    [SerializeField] private string _gameId;
    [SerializeField] private Feedback _feedback;
    
    private const string _debugUrl = "https://pixygon-server.onrender.com/";
    private bool _useDebug = true;

    public bool IsLoggedIn { get; private set; }
    private LoginToken AccountData;
    private async void Start() {
        if (PlayerPrefs.GetInt("RememberMe") == 1) {
            AccountData = await LogIn(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("Password"));
            SaveManager.SettingsSave._user = AccountData.user;
            SaveManager.SettingsSave._isLoggedIn = true;
        }
    }
    public void SetDebug(bool debug) {
        _useDebug = debug;
    }

    public async void StartLogin(string user, string pass, bool rememberMe = false, Action onLogin = null) {
        if (rememberMe) {
            PlayerPrefs.SetInt("RememberMe", 1);
            PlayerPrefs.SetString("Username", user);
            PlayerPrefs.SetString("Password", pass);
            PlayerPrefs.Save();
        }
        AccountData = await LogIn(user, pass);
        SaveManager.SettingsSave._user = AccountData.user;
        SaveManager.SettingsSave._isLoggedIn = true;
        onLogin?.Invoke();
    }

    public async void PatchWaxWallet(string wallet) {
        Debug.Log("Patching wax-wallet");
        var www = await PostVerifiedWWW($"users/{AccountData.user._id}/wax/{wallet}", AccountData.token, "");
        Debug.Log("WaxWallet Patch: " + www.downloadHandler.text);
        AccountData.user = JsonUtility.FromJson<AccountData>(www.downloadHandler.text);
        SaveManager.SettingsSave._user = AccountData.user;
    }
    public async void PatchEthWallet(string wallet) {
        Debug.Log("Patching eth-wallet");
        var www = await PostVerifiedWWW($"users/{AccountData.user._id}/eth/{wallet}", AccountData.token, "");
        Debug.Log("EthWallet Patch: " + www.downloadHandler.text);
    }

    public async void StartLogout() {
        PlayerPrefs.DeleteKey("RememberMe");
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.Save();
        SaveManager.SettingsSave._user = null;
        SaveManager.SettingsSave._isLoggedIn = false;
        AccountData = null;
    }
    public async Task<LoginToken> LogIn(string user, string pass) {
        var www = await PostWWW("auth/login", JsonUtility.ToJson(new LoginData(user, pass)));
        Debug.Log(www.downloadHandler.text);
        IsLoggedIn = true;
        return JsonUtility.FromJson<LoginToken>(www.downloadHandler.text);
    }
    public async void GetUsers() {
        var www = await GetWWW("Users");
    }
    public async void GetFeedback() {
        var www = await GetWWW("client/feedbacks");
    }
    public async void PostHighScore(string game, string user, int score, string detail) {
        var www = await PostWWW("client/highscores", JsonUtility.ToJson(new HighScore(game, user, score, detail)));
        Debug.Log($"highScore: {www.downloadHandler.text}");
    }
    public async void PostFeedback(Feedback feedback) {
        var www = await PostWWW("client/feedbacks", JsonUtility.ToJson(feedback));
        Debug.Log($"Feedback: {www.downloadHandler.text}");
    }

    private async Task<UnityWebRequest> GetWWW(string path) {
        var www = UnityWebRequest.Get(_debugUrl + path);
        www.timeout = 60;
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();
        //_consoleText.text += $"Result: {www.responseCode} | {www.downloadHandler.text}\n";
        return www;
    }
    private async Task<UnityWebRequest> PostWWW(string path, string body)
    {
        var www = UnityWebRequest.Put(_debugUrl + path, body);
        www.timeout = 60;
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();
        //_consoleText.text += $"Result: {www.responseCode} | {www.downloadHandler.text}\n";
        return www;
    }
    private async Task<UnityWebRequest> PostVerifiedWWW(string path, string token, string body)
    {
        var www = UnityWebRequest.Put(_debugUrl + path, body);
        www.timeout = 60;
        www.method = "PATCH";
        www.SetRequestHeader("Authorization", $"Bearer {token}");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();
        //_consoleText.text += $"Result: {www.responseCode} | {www.downloadHandler.text}\n";
        return www;
    }
}

[Serializable]
public class LoginData {
    public string email;
    public string password;

    public LoginData(string email, string pass) {
        this.email = email;
        password = pass;
    }
}
[Serializable]
public class LoginToken {
    public AccountData user;
    public string token;
}
[Serializable]
public class Feedback {
    public string gameId;
    public string title;
    public string feedbackType;
    public string description;
    public int rating;
    public float coordinateX;
    public float coordinateY;
    public float coordinateZ;
    public string area;
}

[Serializable]
public class HighScore {
    public string gameId;
    public string userId;
    public int score;
    public string detail;
    public HighScore(string gameId, string userId, int score, string detail) {
        this.gameId = gameId;
        this.userId = userId;
        this.score = score;
        this.detail = detail;
    }
}