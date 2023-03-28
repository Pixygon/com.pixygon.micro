using System;
using System.Collections.Generic;
using System.Linq;
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
            //var s = FaceplateData.ConvertToJson(MicroController._instance.Faceplates);
            //Debug.Log(s);
            //_faceplateList = JsonUtility.FromJson<FaceplateDataList>(s)._data;
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
            //var s = FaceplateData.ConvertToJson(MicroController._instance.Faceplates);
            if (www.error != null) return;
            Faceplates = JsonUtility.FromJson<FaceplateDataList>(www.downloadHandler.text)._data;
            //_faceplateList = JsonUtility.FromJson<FaceplateDataList>(s)._data;
            _faceplateListLoaded = true;
            Debug.Log("Got faceplates!");
            //Debug.Log(s);
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