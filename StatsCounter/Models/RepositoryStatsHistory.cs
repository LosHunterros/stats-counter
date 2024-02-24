using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace StatsCounter.Models;

public class RepositoryStatsHistory
{
    [Key]
    public int Id { get; set; }
    public string Owner { get; set; }
    public string Languages { get; set; }
    public long Size { get; set; }
    public int PublicRepositories { get; set; }
    public double AvgWatchers { get; set; }
    public double AvgForks { get; set; }
    public DateTime Timestamp { get; set; }
}

