using Pixygon.Micro;
using UnityEngine;

public class FaceplateSetter : MonoBehaviour {
    [SerializeField] private Material _faceMat;
    [SerializeField] private Material _bodyMat;
    [SerializeField] private Material _detailsMat;
    [SerializeField] private Material _buttonsMat;

    [SerializeField] private MeshRenderer[] _buttons;
    [SerializeField] private MeshRenderer[] _shoulders;
    [SerializeField] private MeshRenderer _body;
    [SerializeField] private string _textureKeyword = "BaseColorMap";

    public void SetFaceplate(Faceplate faceplate) {
        var body = new Material(_bodyMat);
        var details = new Material(_detailsMat);
        var buttons = new Material(_buttonsMat);
        var face = new Material(_faceMat);
        body.color = faceplate._color;
        details.color = faceplate._detailColor;
        buttons.color = faceplate._buttonColor;
        face.EnableKeyword(_textureKeyword);
        face.SetTexture($"_{_textureKeyword}", faceplate._tex != null ? faceplate._tex : null);
        face.color = faceplate._useFaceplateColor ? faceplate._faceplate : Color.white;
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
}