using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using NSwag.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookworm.WebAPI.Controllers;

using System.Net;
using System.Text.Json;
using Models;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IRestClient restClient;
    private readonly ILogger<SearchController> logger;

    public SearchController(
        IRestClient restClient,
        ILogger<SearchController> logger
        )
    {
        this.restClient = restClient;
        this.logger = logger;
    }

    [HttpGet]
    [OpenApiOperation("GetSearchResults", "Search")]
    [OpenApiTag("Search")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(SearchResponse), Description = "Successfully retrieved results.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "The input is invalid.")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(void), Description = "An error occurred while processing your request.")]

    /// <param name="query">The search query.</param>
    /// <param name="offset">The offset for pagination.</param>
    /// <param name="limit">The limit for pagination.</param>
    /// <returns>Returns search results based on the query.</returns>
    
    public async Task<IActionResult> SearchForWorks([FromQuery]string query, [FromQuery]int? offset, [FromQuery]int? limit)
    {
        // Create request
        var request = new RestRequest("search.json");
        request.AddParameter("q", query);
        request.AddParameter("fields", "key,cover_i,title,author_name,name");
        request.AddParameter("mode", "everything");

        if (offset.HasValue)
        {
            request.AddParameter("offset", offset.ToString());
        }

        if (limit.HasValue)
        {
            request.AddParameter("limit", limit.ToString());
        }    

        // Add accept header 
        request.AddHeader("Accept", "application/json");

        // Execute request
        var response = await restClient.ExecuteAsync(request);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
        {
            logger.LogError($"Search API call to OpenLibrary failed: {response.StatusDescription ?? response.StatusCode.ToString()}");
            return new StatusCodeResult(((int)response.StatusCode));
        }

        // Process response

        try
        {
            var searchResponse = JsonSerializer.Deserialize<SearchResponse>(response.Content);
            return new OkObjectResult(searchResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to deserialize response from search API: {response.Content}");
            return new StatusCodeResult(500);
        }
    }
}

