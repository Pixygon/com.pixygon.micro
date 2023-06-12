using System.Threading.Tasks;
using Pixygon.DebugTool;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private FaceplateSetter _faceplateSetter;
        private string _faceplateListURL = "https://PixygonMicro.b-cdn.net/Faceplates/Faceplates.json";
        private bool _faceplateListLoaded;
        public FaceplateData[] Faceplates { get; private set; }
        
        public FaceplateData CurrentlyLoadedFaceplate {
            get {
                if(PlayerPrefs.GetInt("Faceplate") >= Faceplates.Length)
                    PlayerPrefs.SetInt("Faceplate", 0);
                return Faceplates.Length != 0 ? Faceplates[PlayerPrefs.GetInt("Faceplate")] : null;
            }
        }
        public void Initialize() {
            UpdateFaceplate();
        }

        public void HideConsole(bool hide) {
            _faceplateSetter.gameObject.SetActive(!hide);
        }


        private async Task LoadFaceplateList() {
            var www = UnityWebRequest.Get(_faceplateListURL);
            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if (www.error != null) return;
            Faceplates = JsonUtility.FromJson<FaceplateDataList>(www.downloadHandler.text)._data;
            _faceplateListLoaded = true;
        }
        
        public async void UpdateFaceplate() {
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