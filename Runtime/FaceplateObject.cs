using System;
using Pixygon.PagedContent;
using UnityEngine;
using UnityEngine.UI;

namespace Pixygon.Micro {
    public class FaceplateObject : PagedContentObject {
        public override void Initialize(object d, int num, Action<object, int> a) {
            base.Initialize(d, num, a);
            var faceplate = d as Faceplate;
            //_text.text = faceplate._title;
            if (faceplate._tex != null) {
                GetComponent<Image>().sprite = Sprite.Create(faceplate._tex, new Rect(0, 0, faceplate._tex.width, faceplate._tex.height), new Vector2(.5f, .5f));
                GetComponent<Image>().color = Color.white;
            } else
                GetComponent<Image>().color = faceplate._faceplate;
        }
    }
}