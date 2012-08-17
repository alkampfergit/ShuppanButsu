using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu.Utils
{
    public static class StringUtils
    {
        public static String Slugify(this String title)
        {
            String normalizedTitle = title.Normalize(NormalizationForm.FormD);
            StringBuilder slug = normalizedTitle
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Select(Char.ToLower)
                .Aggregate(new StringBuilder(), (sb, c) => Char.IsLetterOrDigit(c) ? sb.Append(c) : sb.Append("-"));
            return slug.ToString();
        }
    }
}
