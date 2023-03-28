using System;
using System.Threading.Tasks;
using Pixygon.Micro;
using Pixygon.Saving;
using UnityEngine;
using UnityEngine.Networking;

public class PixygonApi : MonoBehaviour {
    [SerializeField] private string _gameId;
    [SerializeField] private Feedback _feedback;
    
    private const string _debugUrl = "https://pixygon-server.onrender.com/";
    private bool _useDebug = true;

    public bool IsLoggedIn { get; private set; }
    public LoginToken AccountData { get; private set; }
    private async void Start() {
        if (PlayerPrefs.GetInt("RememberMe") != 1) return;
        AccountData = await LogIn(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("Password"));
        if (AccountData == null) return;
        SaveManager.SettingsSave._user = AccountData.user;
        SaveManager.SettingsSave._isLoggedIn = true;
    }
    public void SetDebug(bool debug) {
        _useDebug = debug;
    }

    public async void StartLogin(string user, string pass, bool rememberMe = false, Action onLogin = null, Action<string> onFail = null) {
        if (rememberMe) {
            PlayerPrefs.SetInt("RememberMe", 1);
            PlayerPrefs.SetString("Username", user);
            PlayerPrefs.SetString("Password", pass);
            PlayerPrefs.Save();
        }
        AccountData = await LogIn(user, pass, onFail);
        if (AccountData != null) {
            SaveManager.SettingsSave._user = AccountData.user;
            SaveManager.SettingsSave._isLoggedIn = true;
            MicroController._instance.Home.SetUsernameText(AccountData.user.userName);
        }
        onLogin?.Invoke();
    }
    public async void StartSignup(string user, string email, string pass, bool rememberMe = false, Action onSignup = null, Action<string> onFail = null) {
        if (rememberMe) {
            PlayerPrefs.SetInt("RememberMe", 1);
            PlayerPrefs.SetString("Username", user);
            PlayerPrefs.SetString("Password", pass);
            PlayerPrefs.Save();
        }
        AccountData = await Signup(user,email, pass, onFail);
        if (AccountData != null) {
            SaveManager.SettingsSave._user = AccountData.user;
            SaveManager.SettingsSave._isLoggedIn = true;
            MicroController._instance.Home.SetUsernameText(AccountData.user.userName);
        }
        onSignup?.Invoke();
    }

    public async void PatchWaxWallet(string wallet) {
        var www = await PostVerifiedWWW($"users/{AccountData.user._id}/wax/{wallet}", AccountData.token, "");
        AccountData.user = JsonUtility.FromJson<AccountData>(www.downloadHandler.text);
        SaveManager.SettingsSave._user = AccountData.user;
    }
    public async void PatchEthWallet(string wallet) {
        Debug.Log("Patching eth-wallet");
        var www = await PostVerifiedWWW($"users/{AccountData.user._id}/eth/{wallet}", AccountData.token, "");
        Debug.Log("EthWallet Patch: " + www.downloadHandler.text);
    }
    public async void PatchTezWallet(string wallet) {
        Debug.Log("Patching tez-wallet");
        var www = await PostVerifiedWWW($"users/{AccountData.user._id}/tez/{wallet}", AccountData.token, "");
        Debug.Log("TezWallet Patch: " + www.downloadHandler.text);
    }

    public async Task<Savedata>  GetSave(string gameId, int slot) {
        //.get("/savedata/:gameId/:userId/:slot", verifyToken, getSavedata)
        var www = await GetWWW($"general/savedata/{gameId}/{AccountData.user._id}/{slot}");
        if (!string.IsNullOrWhiteSpace(www.error)) {
            Debug.Log("ERROR!! " + www.error + " and this " + www.downloadHandler.text);
            return null;
        }
        Debug.Log("Savefile retrieved: " + www.downloadHandler.text);
        return JsonUtility.FromJson<Savedata>(www.downloadHandler.text);
    }

    public async Task<Savedata> PostSave(string gameId, int slot, string savedata) {
        //.post("/savedata/:gameId/:userId/:slot/", verifyToken, postSavedata)
        var www = await PostVerifiedWWW($"savedata/savedata/{gameId}/{AccountData.user._id}/{slot}", AccountData.token, savedata);
        if (!string.IsNullOrWhiteSpace(www.error)) {
            Debug.Log("ERROR!! " + www.error + " and this " + www.downloadHandler.text);
            return null;
        }
        Debug.Log("Savefile created: " + www.downloadHandler.text);
        return JsonUtility.FromJson<Savedata>(www.downloadHandler.text);
    }
    
    public async void PatchSave(Savedata savedata) {
        //.patch("/savedata/:id", verifyToken, addSavedata);
        Debug.Log("Patching savegame for " + savedata.gameId);
        var www = await PostVerifiedWWW($"general/savedata/{savedata._id}", AccountData.token, JsonUtility.ToJson(savedata));
        Debug.Log("Savegame Patch: " + www.downloadHandler.text);
    }

    public async void StartLogout() {
        PlayerPrefs.DeleteKey("RememberMe");
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.Save();
        SaveManager.SettingsSave._user = null;
        SaveManager.SettingsSave._isLoggedIn = false;
        AccountData = null;
        IsLoggedIn = false;
    }

    public async Task<LoginToken> LogIn(string user, string pass, Action<string> onFail = null) {
        var www = await PostWWW("auth/login", JsonUtility.ToJson(new LoginData(user, pass)));
        if (!string.IsNullOrWhiteSpace(www.error)) {
            Debug.Log("ERROR!! " + www.error + " and this " + www.downloadHandler.text);
            onFail?.Invoke($"{www.error}\n{www.downloadHandler.text}");
            return null;
        }
        IsLoggedIn = true;
        Debug.Log("Logged in: " + www.downloadHandler.text);
        return JsonUtility.FromJson<LoginToken>(www.downloadHandler.text);
    }
    public async Task<LoginToken> Signup(string user, string email, string pass, Action<string> onFail = null) {
        var www = await PostWWW("auth/register", JsonUtility.ToJson(new SignupData(user, email, pass)));
        if (!string.IsNullOrWhiteSpace(www.error)) {
            Debug.Log("ERROR!! " + www.error + " and this " + www.downloadHandler.text);
            onFail?.Invoke($"{www.error}\n{www.downloadHandler.text}");
            return null;
        }
        IsLoggedIn = true;
        Debug.Log("Signed in: " + www.downloadHandler.text);
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

    private static async Task<UnityWebRequest> GetWWW(string path) {
        var www = UnityWebRequest.Get(_debugUrl + path);
        www.timeout = 60;
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();
        //_consoleText.text += $"Result: {www.responseCode} | {www.downloadHandler.text}\n";
        return www;
    }
    private static async Task<UnityWebRequest> PostWWW(string path, string body) {
        var www = UnityWebRequest.Put(_debugUrl + path, body);
        www.timeout = 30;
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();
        //_consoleText.text += $"Result: {www.responseCode} | {www.downloadHandler.text}\n";
        return www;
    }
    private static async Task<UnityWebRequest> PostVerifiedWWW(string path, string token, string body)
    {
        var www = UnityWebRequest.Put(_debugUrl + path, body);
        www.timeout = 60;
        www.method = "PATCH";
        www.SetRequestHeader("Authorization", $"Bearer {token}");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SendWebRequest();
        while (!www.isDone) await Task.Yield();
        //_consoleText.text += $"Result: {www.responseCode} | {www.downloadHandler.text}\n";
        return www;
    }
}

[Serializable]
public class LoginData {
    public string userName;
    public string password;

    public LoginData(string user, string pass) {
        userName = user;
        password = pass;
    }
}
[Serializable]
public class SignupData {
    public string userName;
    public string email;
    public string password;

    public SignupData(string user, string email, string pass) {
        userName = user;
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
public class Savedata {
    public string _id;
    public string gameId;
    public string userId;
    public int slot;
    public string save;
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