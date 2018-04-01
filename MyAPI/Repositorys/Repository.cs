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
        void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> RemoveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

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

    public class Repository<T> : IRepository<T>
        where T : class
    {
        public IDbConnection Conn { get; private set; }
        public IDapperContext Context { get; private set; }

        public Repository(IDapperContext context)
        {
            Context = context;
            Conn = Context.Connection;
        }

        public virtual void Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Add to DB null entity");
            }
            var res = Conn.Insert(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<int> AddAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Add to DB null entity");
            }
            return Conn.InsertAsync(entity, transaction, commandTimeout);
        }

        public virtual void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }

            Conn.Update(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<bool> UpdateAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Update in DB null entity");
            }

            return Conn.UpdateAsync(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual void Remove(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            Conn.Delete(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<bool> RemoveAsync(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Remove in DB null entity");
            }
            return Conn.DeleteAsync(entity, transaction: transaction, commandTimeout: commandTimeout);
        }

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
            return Conn.GetAsync<T>(id, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual IEnumerable<T> GetAll(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.GetAll<T>(transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.GetAllAsync<T>(transaction: transaction, commandTimeout: commandTimeout);
        }

        //public virtual IEnumerable<T> GetBy(object where = null, object order = null, IDbTransaction transaction = null, int? commandTimeout = null)
        //{
        //    return Conn.GetBy < T > where: where, order: order, transaction: transaction, commandTimeout: commandTimeout);
        //}

        public virtual IEnumerable<T> Exec<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.Query<T>(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<IEnumerable<T>> ExecAsync<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.QueryAsync<T>(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual void Exec(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            Conn.Execute(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }

        public virtual Task<int> ExecAsync(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Conn.ExecuteAsync(sql, param, commandType: commandType, transaction: transaction, commandTimeout: commandTimeout);
        }
    }
}
