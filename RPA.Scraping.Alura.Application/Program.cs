using Microsoft.EntityFrameworkCore;
using RPA.Scraping.Alura.Application;
using RPA.Scraping.Alura.Domain.Entities;
using RPA.Scraping.Alura.Domain.Interfaces;
using RPA.Scraping.Alura.Infra.Data.Context;
using RPA.Scraping.Alura.Infra.Data.Repository;
using RPA.Scraping.Alura.Service.Services;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((hostcontext, services) =>
    {
        services.AddHostedService<Worker>();

        //injecao de dependencias
        string? _connectionString = hostcontext.Configuration.GetConnectionString("Automacao");
        services.AddDbContext<MySqlContext>(options => options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString)), ServiceLifetime.Singleton);
        services.AddSingleton<IBaseRepository<Curso>, BaseRepository<Curso>>();
        services.AddSingleton<IBaseService<Curso>, BaseService<Curso>>();
        services.AddScoped<ISeleniumService, SeleniumService>();

        // seta as configurações do serilog
        Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);

        // cria uma nova instancia do logger
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(hostcontext.Configuration).CreateLogger();

    })
    .Build();

host.Run();
