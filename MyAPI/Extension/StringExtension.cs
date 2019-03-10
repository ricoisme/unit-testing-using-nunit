using System.Linq;
using System.Text;

namespace MyAPI.Extension
{
    public static class StringExtension
    {
        public static string GenerateCacheKey(
            this string controllerName,
            string actionName,
            string methodName,
            string[] parameters = null)
        {
            var builder = new StringBuilder();
            builder.Append(controllerName);
            builder.Append(Const.LinkChar);
            builder.Append(actionName);
            builder.Append(Const.LinkChar);
            builder.Append(methodName);
            parameters?.ToList().ForEach(s =>
            {
                builder.Append(s);
                builder.Append(Const.LinkChar);
            });
            return builder.ToString().TrimEnd(Const.LinkChar);
        }
    }
}
