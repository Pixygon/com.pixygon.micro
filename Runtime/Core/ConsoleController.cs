using System.Threading.Tasks;
using Pixygon.DebugTool;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private FaceplateSetter _faceplateSetter;
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Transform _screenCanvas;
        [SerializeField] private Transform _gameUi;
        [SerializeField] private GameObject _onScreenControls;

        [SerializeField] private FaceplateData _backupFaceplate;
        
        private string _faceplateListURL = "https://PixygonMicro.b-cdn.net/Faceplates/Faceplates.json";
        private bool _faceplateListLoaded = false;
        public FaceplateData[] Faceplates { get; private set; }
        public Transform ScreenCanvas => _screenCanvas;
        public Transform GameUi => _gameUi;

        public bool TestOnScreenControls;
        
        public FaceplateData CurrentlyLoadedFaceplate {
            get {
                if(Faceplates == null) {
                    PlayerPrefs.SetInt("Faceplate", 0);
                    return _backupFaceplate;
                }
                if(PlayerPrefs.GetInt("Faceplate") >= Faceplates.Length) {
                    PlayerPrefs.SetInt("Faceplate", 0);
                }
                return Faceplates.Length != 0 ? Faceplates[PlayerPrefs.GetInt("Faceplate")] : null;
            }
        }
        public void Initialize() {
            SetCanvas();
            UpdateFaceplate();
        }
        private void SetCanvas() {
            #if UNITY_IOS || UNITY_ANDROID
                _onScreenControls.SetActive(true);
                _screenCanvas.GetComponent<CanvasGroup>().interactable = true;
            #else 
                _onScreenControls.SetActive(false);
            #endif
            /*
            if (TestOnScreenControls) {
                _onScreenControls.SetActive(true);
                _screenCanvas.GetComponent<CanvasGroup>().interactable = true;
            }
            else {
                _onScreenControls.SetActive(false);
            }
            */
            _mainCanvas.worldCamera = Camera.main;
        }
        public void HideConsole(bool hide) {
            _faceplateSetter.gameObject.SetActive(!hide);
        }
        private async Task LoadFaceplateList() {
            var www = UnityWebRequest.Get(_faceplateListURL);
            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if (www.error != null) {
                Debug.Log(www.url);
                Log.DebugMessage(DebugGroup.PixygonMicro, "Faceplates error: " + www.error, this);
                return;
            }
            Faceplates = JsonUtility.FromJson<FaceplateDataList>(www.downloadHandler.text)._data;
            _faceplateListLoaded = true;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Faceplates loaded!", this);
        }
        
        public async void UpdateFaceplate() {
            Log.DebugMessage(DebugGroup.PixygonMicro, "Update faceplate!", this);
            if (!_faceplateListLoaded)
                await LoadFaceplateList();
            var plate = CurrentlyLoadedFaceplate;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Set faceplate to " + plate._title, this);
            _faceplateSetter.SetFaceplate(plate);
        }
        public void SetPitch(float f) {
            
        }
        public void SetYaw(float f) {
            
        }
    }
}