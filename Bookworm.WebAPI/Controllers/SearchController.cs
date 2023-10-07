using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookworm.WebAPI.Controllers;

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
    public async Task<IActionResult> Get([FromQuery]string q, [FromQuery]int? offset, [FromQuery]int? limit)
    {
        // Create request
        var request = new RestRequest("search.json");
        request.AddParameter("q", q);
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

