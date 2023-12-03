using RPA.Scraping.Alura.Domain.Entities;
using RPA.Scraping.Alura.Domain.Interfaces;
using RPA.Scraping.Alura.Infra.Data.Context;

namespace RPA.Scraping.Alura.Infra.Data.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly MySqlContext _mySqlContext;

        public BaseRepository(MySqlContext mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public void AtualizarRegistro(TEntity obj)
        {
            _mySqlContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _mySqlContext.SaveChanges();
        }

        public void DeletarRegistro(int id)
        {
            _mySqlContext.Set<TEntity>().Remove(SelecionarRegistroById(id));
        }

        public IList<TEntity> GetAllRegistros()
        {
            return _mySqlContext.Set<TEntity>().ToList();
        }

        public void InserirRegistro(TEntity obj)
        {
            _mySqlContext.Set<TEntity>().Add(obj);
            _mySqlContext.SaveChanges();
        }

        public TEntity SelecionarRegistroById(int id)
        {
            return _mySqlContext.Set<TEntity>().Find(id);
        }
    }
}
