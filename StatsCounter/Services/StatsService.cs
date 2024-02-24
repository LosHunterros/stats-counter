using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using StatsCounter.Models;
using StatsCounter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StatsCounter.Services;

public interface IStatsService
{
    Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner);
}
    
public class StatsService : IStatsService
{
    private readonly IGitHubService _gitHubService;
    private readonly StatsCounterContext _context;

    public StatsService(IGitHubService gitHubService, StatsCounterContext context)
    {
        _gitHubService = gitHubService ?? throw new ArgumentNullException(nameof(gitHubService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner)
    {
        IEnumerable<RepositoryInfo> repositoryInfos = await _gitHubService.GetRepositoryInfosByOwnerAsync(owner);

        IEnumerable<string> languages = repositoryInfos.Select(item => item.Language).Distinct();
        long size = repositoryInfos.Sum(item => item.Size);
        int publicRepositories = repositoryInfos.Count();
        double avgWatchers = repositoryInfos.Average(item => item.Watchers);
        double avgForks = repositoryInfos.Average(item => item.Forks);

        RepositoryStats repositoryStats = new RepositoryStats
        {
            Owner = owner,
            Languages = languages,
            Size = size,
            PublicRepositories = publicRepositories,
            AvgWatchers =avgWatchers,
            AvgForks=avgForks
        };

        RepositoryStatsHistory repositoryStatsHistory = new RepositoryStatsHistory
        {
            Owner = owner,
            Languages = string.Join(", ", repositoryStats.Languages),
            Size = repositoryStats.Size,
            PublicRepositories = repositoryStats.PublicRepositories,
            AvgWatchers = repositoryStats.AvgWatchers,
            AvgForks = repositoryStats.AvgForks,
            Timestamp = DateTime.UtcNow
        };

        if (_context.RepositoryStatsHistories == null)
        {
            throw new ArgumentNullException(nameof(_context.RepositoryStatsHistories));
        }

        _context.RepositoryStatsHistories.Add(repositoryStatsHistory);
        await _context.SaveChangesAsync();

        return repositoryStats;
    }
}