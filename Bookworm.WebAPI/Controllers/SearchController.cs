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
    public async Task<IActionResult> Get([FromQuery]string q)
    {
        // Create request
        var request = new RestRequest("search.json");
        request.AddParameter("q", q);
        request.AddParameter("fields", "key,cover_i,title,author_name,name");
        request.AddParameter("mode", "everything");

        // Add accept header 
        request.AddHeader("Accept", "application/json");

        // Execute request
        var response = await restClient.ExecuteAsync(request);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
        {
            // Should let the client know that something wen wrong
            return new StatusCodeResult(500);
        }

        // Process response
        var searchResponse = JsonSerializer.Deserialize<SearchResponse>(response.Content);

        return new OkObjectResult(searchResponse);
    }
}

