using System.Text;

namespace Sib.UmbracoStaticFileGenerator.StaticFileGenerator
{
    public static class Helpers
    {
        public static string RemoveAllSlashes(this string inputString)
        {
            var replacements = new[] { "\\", "/" };
            var output = new StringBuilder(inputString);
            foreach (var r in replacements)
                output.Replace(r, string.Empty);

            return output.ToString();
        } 
    }
}