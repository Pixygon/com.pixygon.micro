using System;
using System.Threading.Tasks;
using Pixygon.Core;
using UnityEngine;
using UnityEngine.Networking;

public class PixygonApi : MonoBehaviour {
    [SerializeField] private string _userName;
    [SerializeField] private string _password;
    [SerializeField] private string _gameId;
    [SerializeField] private Feedback _feedback;
    
    private const string _debugUrl = "https://pixygon-server.onrender.com/";
    private bool _useDebug = true;

    private LoginToken _data;

    private async void Start() {
        _data = await LogIn(_userName, _password);
        //PostHighScore(_gameId ,_data.user._id, UnityEngine.Random.Range(0, 100000), "Some level");
        //PostFeedback(_feedback);
    }
    public void SetDebug(bool debug) {
        _useDebug = debug;
    }
    public async Task<LoginToken> LogIn(string user, string pass) {
        var www = await PostWWW("auth/login", JsonUtility.ToJson(new LoginData(user, pass)));
        return(JsonUtility.FromJson<LoginToken>(www.downloadHandler.text));
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
public class AccountData {
    public string _id;
    public string userName;
    //public string password;
    public string email;
    public string picturePath;
    public string[] friends;
    public string[] transactions;
    public string role;
    public int viewedProfile;
    public int impressions;
}

[Serializable]
public class Feedback
{
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