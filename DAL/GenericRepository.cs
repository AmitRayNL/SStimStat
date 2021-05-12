using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SStimStat.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal raysstim_statsEntities context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(raysstim_statsEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity GetByID(object id1, object id2)
        {
            return dbSet.Find(id1, id2);
        }

        public virtual TEntity GetByID(object id1, object id2, object id3)
        {
            return dbSet.Find(id1, id2, id3);
        }

        /// <summary>
        /// Added by Amit
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual TEntity GetByKeys(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Added by Amit
        /// </summary>
        /// <param name="keyValues"></param>
        public virtual void Delete(params object[] keyValues)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(keyValues);
                //TEntity entityToDelete = dbSet.Find(3814, "LL0010", 1, 1);
                Delete(entityToDelete);
            }
            catch (ArgumentException aex)
            {
                string test = "";
            }
        }

        public virtual void Delete(object id)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(id);
                //TEntity entityToDelete = dbSet.Find(3814, "LL0010", 1, 1);
                Delete(entityToDelete);
            }
            catch (ArgumentException aex)
            {
                string test = "";
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            //dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Update(string updateSqlQuery)
        {
            context.Database.ExecuteSqlCommand(updateSqlQuery);
        }
    }
}