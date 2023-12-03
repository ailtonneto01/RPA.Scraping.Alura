using RPA.Scraping.Alura.Domain.Entities;
using RPA.Scraping.Alura.Domain.Interfaces;

namespace RPA.Scraping.Alura.Service.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        private readonly IBaseRepository<TEntity> _repository;

        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public void InserirRegistro(TEntity entity)
        {
            _repository.InserirRegistro(entity);
        }
    }
}
