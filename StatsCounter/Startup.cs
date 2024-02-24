using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using StatsCounter.Extensions;
using StatsCounter.Services;
using StatsCounter.Data;

namespace StatsCounter;

public class Startup
{
    private readonly IConfiguration _configuration;
        
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Add dbcontext
        services.AddDbContext<StatsCounterContext>(Options =>
        {
            Options.UseSqlite("Data Source=Sqlite.db");
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo 
            {
                Title = "StatsCounter API",
                Version = "v1"
            });
        });

        services.AddGitHubService(new Uri(_configuration["GitHubSettings:BaseApiUrl"]))
            .AddTransient<IStatsService, StatsService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StatsCounter API v1");
                c.RoutePrefix = string.Empty;
            });
        }
        else
        {
            app.UseHttpsRedirection();

        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}