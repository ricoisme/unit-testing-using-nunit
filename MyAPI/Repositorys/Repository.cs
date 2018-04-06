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
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            var res = Conn.Insert(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual async Task<int> AddAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Add to DB null entity");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return await Conn.InsertAsync(entity, transaction, commandTimeout);
        }

        public virtual long BulkAdd(IList<T> entities, IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.Insert(entities, transaction, commandTimeout);
        }
        public virtual async Task<long> BulkAddAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return await Conn.InsertAsync(entities, transaction, commandTimeout);
        }

        #endregion


        #region update
        public virtual void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            Conn.Update(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual async Task<bool> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return await Conn.UpdateAsync(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual bool BulkUpdate(IList<T> entities, IDbTransaction transaction = null,
           int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.Update(entities, transaction, commandTimeout);
        }
        public virtual async Task<bool> BulkUpdateAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return await Conn.UpdateAsync(entities, transaction, commandTimeout);
        }

        #endregion


        #region delete

        public virtual void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            Conn.Delete(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<bool> RemoveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.DeleteAsync(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual bool BulkRemove(IList<T> entities, IDbTransaction transaction = null,
          int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.Delete(entities, transaction, commandTimeout);
        }
        public virtual async Task<bool> BulkRemoveAsync(IList<T> entities, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities", "Add to DB null entities");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return await Conn.DeleteAsync(entities, transaction, commandTimeout);
        }

        #endregion


        #region select

        public virtual T GetByKey(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return Conn.Get<T>(id, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<T> GetByKeyAsync(object id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.GetAsync<T>(id, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual IEnumerable<T> GetAll(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.GetAll<T>(transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.GetAllAsync<T>(transaction: transaction, commandTimeout: commandTimeout);
        }

        //public virtual IEnumerable<T> GetBy(object where = null, object order = null, IDbTransaction transaction = null, int? commandTimeout = null)
        //{
        //    return Conn.GetBy < T > where: where, order: order, transaction: transaction, commandTimeout: commandTimeout);
        //}

        #endregion


        #region exec

        public virtual IEnumerable<T> Exec<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.Query<T>(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<IEnumerable<T>> ExecAsync<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.QueryAsync<T>(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual void Exec(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            Conn.Execute(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<int> ExecAsync(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (Conn == null)
                Conn = Context.Connection;
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            return Conn.ExecuteAsync(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        #endregion
    }
}
