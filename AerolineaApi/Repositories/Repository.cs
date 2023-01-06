using AerolineaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AerolineaApi.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly sistem21_aerolineaContext context;

        public Repository(sistem21_aerolineaContext context)
        {
            this.context = context;
        }

        public DbSet<T> Get()
        {
            return context.Set<T>();
        }

        public T? Get(object id)
        {
            return context.Find<T>(id);
        }
        public void Insert(T entidad)
        {
            context.Add(entidad);
            context.SaveChanges();
        }

        public void Update(T entidad)
        {
            context.Update(entidad);
            context.SaveChanges();
        }

        public void Delete(T entidad)
        {
            context.Remove(entidad);
            context.SaveChanges();
        }

    }
}
