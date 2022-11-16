using System;
using Pixygon.PagedContent;

namespace Pixygon.Micro {
    public class FaceplateObject : PagedContentObject {
        public override void Initialize(object d, int num, Action<object, int> a) {
            base.Initialize(d, num, a);
            _text.text = (d as Faceplate)._title;
        }
    }
}