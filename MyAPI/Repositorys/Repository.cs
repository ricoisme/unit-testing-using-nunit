using Dapper;
using Dapper.Contrib.Extensions;
using MyAPI.Infarstructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyAPI.Repositorys
{
    public interface IRepository<T>
        where T : class
    {
        IDbConnection Conn { get; }
        IDapperContext Context { get; }

        void Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<int> AddAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        long BulkAdd(IList<T> entities, IDbTransaction transaction = null,
            int? commandTimeout = null);
        Task<long> BulkAddAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null);
        void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        bool BulkUpdate(IList<T> entities, IDbTransaction transaction = null,
           int? commandTimeout = null);
        Task<bool> BulkUpdateAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null);
        void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> RemoveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        bool BulkRemove(IList<T> entities, IDbTransaction transaction = null,
          int? commandTimeout = null);
        Task<bool> BulkRemoveAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null);
        T GetByKey(object key, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<T> GetByKeyAsync(object id, IDbTransaction transaction = null, int? commandTimeout = null);

        IEnumerable<T> GetAll(IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction = null, int? commandTimeout = null);
        //IEnumerable<T> GetBy(object where = null, object order = null, IDbTransaction transaction = null, int? commandTimeout = null);

        IEnumerable<T> Exec<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<IEnumerable<T>> ExecAsync<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null);

        void Exec(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<int> ExecAsync(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null);
    }

    public partial class Repository<T> : IRepository<T>
        where T : class
    {
        public IDbConnection Conn { get; private set; }
        public IDapperContext Context { get; private set; }

        public Repository(IDapperContext context)
        {
            Context = context;
            Conn = Context.Connection;
        }

        #region insert
        public virtual void Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Add to DB null entity");
            }
            var affectedRows = Conn.Insert(entity, transaction, commandTimeout);
        }

        public virtual async Task<int> AddAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Add to DB null entity");
            }
            return await Conn.InsertAsync(entity, transaction, commandTimeout);
        }

        public virtual long BulkAdd(IList<T> entities, IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (transaction == null)
            {
                Conn.Open();
                using (transaction = Conn.BeginTransaction())
                {
                    var affectedRows = Conn.Insert(entities, transaction, commandTimeout);
                    transaction.Commit();
                    return affectedRows;
                }
            }
            return 0;
        }
        public virtual async Task<long> BulkAddAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (transaction == null)
            {
                Conn.Open();
                using (transaction = Conn.BeginTransaction())
                {
                    var affectedRows = await Conn.InsertAsync(entities, transaction, commandTimeout);
                    transaction.Commit();
                    return affectedRows;
                }
            }
            return 0;
        }

        #endregion


        #region update
        public virtual void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }
            Conn.Update(entity, transaction, commandTimeout);
        }

        public virtual async Task<bool> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }
            return await Conn.UpdateAsync(entity, transaction, commandTimeout);
        }

        public virtual bool BulkUpdate(IList<T> entities, IDbTransaction transaction = null,
           int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (transaction == null)
            {
                Conn.Open();
                using (transaction = Conn.BeginTransaction())
                {
                    var result = Conn.Update(entities, transaction, commandTimeout);
                    transaction.Commit();
                    return result;
                }
            }
            return false;
        }
        public virtual async Task<bool> BulkUpdateAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (transaction == null)
            {
                Conn.Open();
                using (transaction = Conn.BeginTransaction())
                {
                    var result = await Conn.UpdateAsync(entities, transaction, commandTimeout);
                    transaction.Commit();
                    return result;
                }
            }
            return await Task.FromResult(false);
        }

        #endregion


        #region delete

        public virtual void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            Conn.Delete(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual async Task<bool> RemoveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            return await Conn.DeleteAsync(entity, transaction, commandTimeout);
        }

        public virtual bool BulkRemove(IList<T> entities, IDbTransaction transaction = null,
          int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (transaction == null)
            {
                Conn.Open();
                using (transaction = Conn.BeginTransaction())
                {
                    var result = Conn.Delete(entities, transaction, commandTimeout);
                    transaction.Commit();
                    return result;
                }
            }
            return false;
        }
        public virtual async Task<bool> BulkRemoveAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (transaction == null)
            {
                Conn.Open();
                using (transaction = Conn.BeginTransaction())
                {
                    var result = await Conn.DeleteAsync(entities, transaction, commandTimeout);
                    transaction.Commit();
                    return result;
                }
            }
            return await Task.FromResult(false);
        }

        #endregion


        #region select

        public virtual T GetByKey(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return Conn.Get<T>(id, transaction, commandTimeout);
        }

        public virtual Task<T> GetByKeyAsync(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return Conn.GetAsync<T>(id, transaction, commandTimeout);
        }

        public virtual IEnumerable<T> GetAll(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.GetAll<T>(transaction, commandTimeout);
        }

        public virtual Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.GetAllAsync<T>(transaction, commandTimeout);
        }

        //public virtual IEnumerable<T> GetBy(object where = null, object order = null, IDbTransaction transaction = null, int? commandTimeout = null)
        //{
        //    return Conn.GetBy < T > where: where, order: order, transaction: transaction, commandTimeout: commandTimeout);
        //}

        #endregion


        #region exec

        public virtual IEnumerable<T> Exec<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            sql = $"SET XACT_ABORT ON; {sql}";
            return Conn.Query<T>(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<IEnumerable<T>> ExecAsync<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            sql = $"SET XACT_ABORT ON; {sql}";
            return Conn.QueryAsync<T>(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual void Exec(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            sql = $"SET XACT_ABORT ON; {sql}";
            Conn.Execute(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<int> ExecAsync(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            sql = $"SET XACT_ABORT ON; {sql}";
            return Conn.ExecuteAsync(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        #endregion
    }
}
