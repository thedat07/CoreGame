using UnityEngine;

namespace YNL.Utilities.Extensions
{
    public static class MDebug
    {
        public static void Log(object message)
            => UnityEngine.Console.Log($"<color=#9EFFF9><b>ⓘ Log:</b></color> {message}");

        public static void Warning(object message)
            => UnityEngine.Console.LogWarning($"<color=#FFE045><b>⚠ Warning:</b></color> {message}");

        public static void Caution(object message)
            => UnityEngine.Console.Log($"<color=#FF983D><b>⚠ Caution:</b></color> {message}");

        public static void Action(object message)
            => UnityEngine.Console.Log($"<color=#EC82FF><b>▶ Action:</b></color> {message}");

        public static void Notify(object message)
            => UnityEngine.Console.Log($"<color=#FFCD45><b>▶ Notification:</b></color> {message}");

        public static void Error(object message)
            => UnityEngine.Console.LogError($"<color=#FF3C2E><b>⚠ Error:</b></color> {message}");

        public static void Custom(string custom = "Custom", object message = null, string color = "#63ff9a")
            => UnityEngine.Console.Log($"<color={color}><b>✔ {custom}:</b></color> {message}");

        public static void Debug(object message, Color color)
            => UnityEngine.Console.Log($"<color={color.ToHex()}><b>⚠ Debug:</b></color> {message}");
    }
}