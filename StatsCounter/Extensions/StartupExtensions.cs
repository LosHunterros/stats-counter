using Microsoft.Extensions.DependencyInjection;
using StatsCounter.Services;
using System;

namespace StatsCounter.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddGitHubService(
        this IServiceCollection services, Uri baseApiUrl)
    {
        services.AddHttpClient<IGitHubService, GitHubService>(c =>
        {
            c.BaseAddress = baseApiUrl;
            c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            c.DefaultRequestHeaders.Add("User-Agent", "StatsCounter");
        });
        return services;
    }
}