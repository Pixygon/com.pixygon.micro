using System;
using System.Threading.Tasks;
using Pixygon.DebugTool;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixygon.Micro
{
    public class ConsoleController : MonoBehaviour {
        [SerializeField] private FaceplateSetter _faceplateSetter;
        private string _faceplateListURL = "https://pixygon.b-cdn.net/faceplates.json";
        private bool _faceplateListLoaded;
        private Faceplate[] _faceplateList;
        public Faceplate CurrentlyLoadedFaceplate {
            get {
                if(PlayerPrefs.GetInt("Faceplate") >= _faceplateList.Length)
                    PlayerPrefs.SetInt("Faceplate", 0);
                return _faceplateList.Length != 0 ? _faceplateList[PlayerPrefs.GetInt("Faceplate")] : null;
            }
        }
        public void Initialize() {
            Debug.Log(JsonUtility.ToJson(MicroController._instance.Faceplates));
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
            _faceplateList = JsonUtility.FromJson<Faceplate[]>(www.downloadHandler.text);
            _faceplateListLoaded = true;
        }
        
        public async void UpdateFaceplate() {
            if (!_faceplateListLoaded)
                await LoadFaceplateList();
            var plate = CurrentlyLoadedFaceplate;
            Log.DebugMessage(DebugGroup.PixygonMicro, "Set faceplate to " + plate._title, this);
            _faceplateSetter.SetFaceplate(plate);
        }

        public void SetZoom(float f) {
            MicroController._instance.SetZoom(f);
        }
    }
}