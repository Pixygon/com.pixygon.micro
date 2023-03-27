using System.Threading.Tasks;
using Pixygon.Micro;
using UnityEngine;
using UnityEngine.Networking;

public class FaceplateSetter : MonoBehaviour {
    [SerializeField] private Material _faceMat;
    [SerializeField] private Material _bodyMat;
    [SerializeField] private Material _detailsMat;
    [SerializeField] private Material _buttonsMat;

    [SerializeField] private MeshRenderer[] _buttons;
    [SerializeField] private MeshRenderer[] _shoulders;
    [SerializeField] private MeshRenderer _body;
    [SerializeField] private string _textureKeyword = "BaseColorMap";
    private string url = "https://PixygonMicro.b-cdn.net/Faceplates/";

    public async void SetFaceplate(Faceplate faceplate) {
        var body = new Material(_bodyMat);
        var details = new Material(_detailsMat);
        var buttons = new Material(_buttonsMat);
        var face = await SetTexture(_faceMat, faceplate);
        body.color = faceplate._color;
        details.color = faceplate._detailColor;
        buttons.color = faceplate._buttonColor;
        foreach (var b in _buttons) {
            b.material = buttons;
        }
        foreach (var b in _shoulders) {
            b.material = details;
        }

        var mats = new [] {
            body, face, _body.sharedMaterials[2]
        };
        _body.materials = mats;
    }
    private async Task<Material> SetTexture(Material m, Faceplate faceplate) {
        var face = new Material(_faceMat);
        face.EnableKeyword(_textureKeyword);
        if (faceplate._getImagesFromURL) {
            var www = UnityWebRequestTexture.GetTexture($"{url}{faceplate._textureURL}.png");
            www.SendWebRequest();
            while(!www.isDone)
                await Task.Yield();
            if (www.error != null) return null;
            face.SetTexture($"_{_textureKeyword}",DownloadHandlerTexture.GetContent(www));
        } //else
          //  face.SetTexture($"_{_textureKeyword}", faceplate._tex != null ? faceplate._tex : null);
        face.color = faceplate._useFaceplateColor ? faceplate._faceplate : Color.white;
        return face;
    }
}