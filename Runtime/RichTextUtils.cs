namespace CippSharp.Core.DeCa
{
    using Color = UnityEngine.Color;
    using ColorUtility = UnityEngine.ColorUtility;

    public static class RichTextUtils
    {
        /// <summary>
        /// Set the string to that color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string OfColor(string value, Color color)
        {
            return string.Format("<color=#{1}>{0}</color>", value, ColorUtility.ToHtmlStringRGBA(color));
        }
    }
}
