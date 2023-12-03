using RPA.Scraping.Alura.Domain.Entities;

namespace RPA.Scraping.Alura.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        void InserirRegistro(TEntity obj);
        void AtualizarRegistro(TEntity obj);
        void DeletarRegistro(int obj);
        IList<TEntity> GetAllRegistros();
        TEntity SelecionarRegistroById(int id);
    }
}
