
namespace CippSharp.Core.DeCa
{
    using Color = UnityEngine.Color;
    
    internal static class RichTextExtensions
    {
        /// <summary>
        /// Set the string to that color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string OfColor(this string value, Color color)
        {
            return RichTextUtils.OfColor(value, color);
        }
    }
}
