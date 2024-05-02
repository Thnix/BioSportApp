using System.Globalization;
using System.Text;

namespace BioSportApp.Common.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
