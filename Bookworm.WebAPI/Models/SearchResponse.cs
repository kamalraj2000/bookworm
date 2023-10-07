using System;
using System.Text.Json.Serialization;

namespace Bookworm.WebAPI.Models;

public class SearchResponse
{
    [JsonPropertyName("numFound")]
    public int NumFound { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("numFoundExact")]
    public bool NumFoundExact { get; set; }

    [JsonPropertyName("docs")]
    public Work[] Works { get; set; } = default!;

    [JsonPropertyName("q")]
    public string Query { get; set; } = default!;

    [JsonPropertyName("offset")]
    public int? Offset { get; set; } = default!;
}

