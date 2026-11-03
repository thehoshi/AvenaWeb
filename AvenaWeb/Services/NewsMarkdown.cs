using System.Net;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace AvenaWeb.Services;

public static class NewsMarkdown
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .DisableHtml()
        .Build();

    public static string ToHtml(string markdown)
    {
        var document = Markdown.Parse(markdown ?? string.Empty, Pipeline);

        foreach (var link in document.Descendants<LinkInline>())
        {
            if (!IsSafeDestination(link.Url, link.IsImage))
                link.Url = null;
        }

        return Markdown.ToHtml(document, Pipeline);
    }

    private static bool IsSafeDestination(string? destination, bool isImage)
    {
        if (string.IsNullOrWhiteSpace(destination))
            return false;

        var value = WebUtility.HtmlDecode(destination).Trim();
        if (value.StartsWith("#", StringComparison.Ordinal))
            return true;

        if (value.StartsWith("/", StringComparison.Ordinal))
            return !value.StartsWith("//", StringComparison.Ordinal) && !value.Contains('\\');

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            return uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                || uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                || (!isImage && uri.Scheme.Equals(Uri.UriSchemeMailto, StringComparison.OrdinalIgnoreCase));
        }

        return !value.Contains(':') && !value.StartsWith("//", StringComparison.Ordinal) && !value.Contains('\\');
    }
}
