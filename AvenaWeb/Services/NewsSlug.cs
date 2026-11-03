using System.Text;

namespace AvenaWeb.Services;

public static class NewsSlug
{
    public static string FromTitle(string title)
    {
        var normalized = (title ?? string.Empty).Normalize(NormalizationForm.FormKC).ToLowerInvariant();
        var slug = new StringBuilder(normalized.Length);
        var needsSeparator = false;

        foreach (var character in normalized)
        {
            if (char.IsLetterOrDigit(character))
            {
                if (needsSeparator && slug.Length > 0)
                    slug.Append('-');

                slug.Append(character);
                needsSeparator = false;
            }
            else
            {
                needsSeparator = slug.Length > 0;
            }
        }

        if (slug.Length == 0)
            return "article";

        var value = slug.ToString();
        if (value.All(char.IsDigit))
            value = $"article-{value}";

        return value.Length <= 180 ? value : value[..180].TrimEnd('-');
    }

    public static bool IsValid(string slug) =>
        !string.IsNullOrWhiteSpace(slug)
        && slug.Length <= 180
        && !slug.All(char.IsDigit)
        && slug[0] != '-'
        && slug[^1] != '-'
        && slug.All(character => char.IsLetterOrDigit(character) || character == '-');
}
