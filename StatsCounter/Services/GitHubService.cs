using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StatsCounter.Models;

namespace StatsCounter.Services;

public interface IGitHubService
{
    Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner);
}
    
public class GitHubService : IGitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"https://api.github.com/users/{owner}/repos");

        List<RepositoryInfo> repositoryInfos = await response.Content.ReadFromJsonAsync<List<RepositoryInfo>>();

        return repositoryInfos;
    }
}