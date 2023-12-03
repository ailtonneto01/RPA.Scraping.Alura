using RPA.Scraping.Alura.Domain.Entities;

namespace RPA.Scraping.Alura.Domain.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        void InserirRegistro(TEntity entity);
    }
}
