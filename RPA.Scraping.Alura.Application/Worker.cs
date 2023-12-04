using RPA.Scraping.Alura.Domain.Entities;
using RPA.Scraping.Alura.Domain.Interfaces;

namespace RPA.Scraping.Alura.Application
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseService<Curso> _baseSearchService;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IBaseService<Curso> baseSearchService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _baseSearchService = baseSearchService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("robo iniciado: {time}", DateTimeOffset.Now);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
            
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            ISeleniumService _seleniumService = scope.ServiceProvider.GetRequiredService<ISeleniumService>();

            try
            {
                Console.Clear();
                Console.WriteLine("Informe o curso a ser pesquisado:");
                string? cursoPesquisado = Console.ReadLine();

                if (!string.IsNullOrEmpty(cursoPesquisado))
                {
                    if (_seleniumService.IniciarChromeDriver())
                    {
                        List<Curso> cursos = _seleniumService.GetCursos(cursoPesquisado);

                        if(cursos.Any())
                        {
                            foreach (var curso in cursos)
                            {
                                _baseSearchService.InserirRegistro(curso);

                                Console.WriteLine($"TERMO PESQUISADO: {curso.TermoPesquisado} | " +
                                                  $"TITULO: {curso.Titulo} | " +
                                                  $"PROFESSOR: {curso.Professor} | " +
                                                  $"CARGA HORARIA: {curso.CargaHoraria}");
                            }
                        }

                        _logger.LogInformation($"Total cursos encontrados: {cursos.Count}");
                    }
                }
                else
                {
                    Console.WriteLine("Para efetuar a pesquisa é necessário informar um curso: ");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro Geral - {ex.Message}");
            }
            finally 
            {
                _seleniumService.FinalizarChromeDriver();
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}