using RPA.Scraping.Alura.Domain.Entities;

namespace RPA.Scraping.Alura.Domain.Interfaces
{
    public interface ISeleniumService
    {
        bool IniciarChromeDriver();

        void FinalizarChromeDriver();

        List<Curso> GetCursos(string pesquisa);
    }
}
