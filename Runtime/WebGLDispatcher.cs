using System.Runtime.InteropServices;

namespace  Pixygon.Micro {
    public static class WebGLDispatcher {
#if UNITY_WEBGL
    [DllImport("__Internal")]
    public static extern void Login();
#endif
    }
}