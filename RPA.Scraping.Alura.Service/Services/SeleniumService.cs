using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RPA.Scraping.Alura.Domain.Entities;
using RPA.Scraping.Alura.Domain.Interfaces;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace RPA.Scraping.Alura.Service.Services
{
    public class SeleniumService : ISeleniumService
    {
        private IWebDriver _driver;
        private const string URLBASE = "https://www.alura.com.br/";
        private readonly ILogger<SeleniumService> _logger;

        public SeleniumService(ILogger<SeleniumService> logger)
        {
            _logger = logger;
        }

        public bool IniciarChromeDriver()
        {
            try
            {

                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();

                chromeDriverService.HideCommandPromptWindow = true;

                ChromeOptions options = new()
                {
                    PageLoadStrategy = PageLoadStrategy.Normal
                };

                options.AddArgument("no-sandbox");
                options.AddArgument("--start-maximized");
                options.AddArgument("--profile-directory=Default");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-web-security");
                options.AddExcludedArgument("enable-logging");
                
                options.Proxy = new Proxy { Kind = ProxyKind.System };

                _driver = new ChromeDriver(chromeDriverService, options);
                _driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(1);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao iniciar chromedriver - {ex.Message}");
                throw;
            }
        }

        private bool AcessarPaginaInicial()
        {
            try
            {
                Navigate(URLBASE);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar pagina inicial - {ex.Message}");
            }

            return false;
        }


        public List<Curso> GetCursos(string pesquisa)
        {
            List<Curso> listaCursos = new();

            try
            {
                if (AcessarPaginaInicial())
                {
                    IWebElement? barraBusca = IsElementPresent(By.Id("header-barraBusca-form-campoBusca"));

                    if (barraBusca is not null)
                    {
                        barraBusca.SendKeys(pesquisa + Keys.Enter);

                        Thread.Sleep(TimeSpan.FromMilliseconds(500));

                        if (IsCursoEncontrado())
                        {
                            HtmlNodeCollection paginas = GetPaginasEncontradas();

                            foreach (var pagina in paginas)
                            {
                                if (pagina.Attributes["class"].Value != "paginationLink busca-botao-selecionado")
                                {
                                    Navigate($"https://www.alura.com.br/busca?pagina={pagina.InnerText}&query={pesquisa}");
                                    Thread.Sleep(TimeSpan.FromSeconds(1));
                                }

                                ReadOnlyCollection<IWebElement> cursosEncontrados = GetCursosEncontrados();

                                if(cursosEncontrados.Any())
                                {
                                    foreach (var c in cursosEncontrados)
                                    {
                                        Curso curso = new()
                                        {
                                            TermoPesquisado = pesquisa,
                                            Titulo = GetTituloCurso(c),
                                            Descricao = GetDescricaoCurso(c),
                                            UrlCurso = GetLinkCurso(c),
                                            LastUpdate = DateTime.Now.ToString()
                                        };

                                        GetProfessorAndCargaHoraria(ref curso);
                                        listaCursos.Add(curso);
                                    }

                                    return listaCursos;
                                }
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"Nenhum curso encontrado para - {pesquisa}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter lista de cursos encontrados - {ex.Message}");
            }

            return new List<Curso>();
        }

        private void GetProfessorAndCargaHoraria(ref Curso curso)
        {
            if (!string.IsNullOrEmpty(curso.UrlCurso))
            {
                _driver.SwitchTo().NewWindow(WindowType.Tab);

                Thread.Sleep(TimeSpan.FromMilliseconds(250));

                _driver.Navigate().GoToUrl(curso.UrlCurso);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                curso.CargaHoraria = GetCargaHoraria();

                curso.Professor = GetNomeProfessor();

                _driver.Close();

                Thread.Sleep(TimeSpan.FromMilliseconds(250));

                _driver.SwitchTo().Window(_driver.WindowHandles.First());
                
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
        }


        private string GetCargaHoraria()
        {
            try
            {
                return _driver.FindElement(By.XPath("//p[@class='courseInfo-card-wrapper-infos']")).Text;
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }

        private string GetNomeProfessor()
        {
            try
            {
                return _driver.FindElement(By.ClassName("instructor-title--name")).Text;
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }

            
        }

        private string GetTituloCurso(IWebElement element)
        {
            return element.FindElement(By.ClassName("busca-resultado-nome")).Text;
        }

        private string GetDescricaoCurso(IWebElement element)
        {
            return element.FindElement(By.ClassName("busca-resultado-descricao")).Text;
        }

        private string GetLinkCurso(IWebElement element)
        {
            return element.FindElement(By.ClassName("busca-resultado-link")).GetAttribute("href");
        }

        private ReadOnlyCollection<IWebElement> GetCursosEncontrados()
        {
            return _driver.FindElements(By.XPath("//li[@class='busca-resultado']"));
        }

        private HtmlNodeCollection GetPaginasEncontradas()
        {
            HtmlNodeCollection paginas = HtmlDoc().DocumentNode.SelectNodes("//nav[@class='busca-paginacao-links']//a");

            if(paginas is not null)
            {
                return paginas;
            }
            else
            {
                throw new Exception("Paginas do curso nao encontradas");
            }
        }

        private bool IsCursoEncontrado()
        {
            HtmlNode divResult = HtmlDoc().DocumentNode.SelectSingleNode("//div[@class='search-noResult search-noResult--visible']");

            if(divResult is null)
            {
                return true;
            }

            return false;
        }

        private HtmlDocument HtmlDoc()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(_driver.PageSource);
            return doc;
        }

        private IWebElement? IsElementPresent(By by)
        {
            try
            {
                return _driver.FindElement(by); 
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        private void Navigate(string url)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao efetuar navegacao - {ex.Message}");
            }
        }

        public void FinalizarChromeDriver()
        {
            try
            {
                var processos = Process.GetProcesses().Where(p => p.ProcessName == "chromedriver");

                foreach (var processo in processos)
                {
                    processo.Kill(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao finalizar drivers - ERRO: {ex.Message}");
            }
        }
    }
}
