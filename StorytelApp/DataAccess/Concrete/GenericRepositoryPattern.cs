using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using StorytelApp.DataAccess.Abstract;
using StorytelApp.DataAccess.Context;

namespace StorytelApp.DataAccess.Concrete
{
    public class GenericRepositoryPattern<T> : IGenericRepositoryPattern<T> where T : class
    {
        protected DataClasses1DataContext _context;

        public GenericRepositoryPattern()
        {
            _context = new DataClasses1DataContext();
        }

        public IQueryable<T> GetAll() => from entity in _context.GetTable<T>() select entity;

        public IQueryable<T> Query(Expression<Func<T, bool>> exp) => _context.GetTable<T>().Where(exp);

        public void Insert(T entity)
        {
            _context.GetTable<T>().InsertOnSubmit(entity);
            SubmitChanges();
        }

        public void Delete(T entity)
        {
            _context.GetTable<T>().DeleteOnSubmit(entity);
            SubmitChanges();
        }

        public void Update(T entity)
        {
            _context.GetTable<T>().Attach(entity);
            SubmitChanges();
        }

        public void SubmitChanges() => _context.SubmitChanges();
    }
}
